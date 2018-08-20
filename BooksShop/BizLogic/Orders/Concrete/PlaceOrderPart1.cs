using BizDbAccess;
using BizLogic.GenericInterfaces;
using DataLayer.Entities;

namespace BizLogic.Orders.Concrete
{
    public class PlaceOrderPart1 : BizActionErrors, IPlaceOrderPart1
    {
        private readonly IPlaceOrderDbAccess dbAccess;

        public PlaceOrderPart1(IPlaceOrderDbAccess dbAccess)
        {
            this.dbAccess = dbAccess;
        }

        public Part1ToPart2Dto Action(PlaceOrderInDto dto)
        {
            if (!dto.AcceptTAndCs)
            {
                AddError("You must accept the T&Cs to place an order.");
            }

            if (dto.LineItems.Count == 0)
            {
                AddError("No items in your basket.");
            }

            Order order = new Order { CustomerName = dto.UserId };

            if (!HasErrors)
            {
                dbAccess.Add(order);
            }

            return new Part1ToPart2Dto(dto.LineItems, order);
        }
    }
}
