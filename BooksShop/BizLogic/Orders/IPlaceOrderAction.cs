using BizLogic.GenericInterfaces;
using DataLayer.Entities;

namespace BizLogic.Orders
{
    public interface IPlaceOrderAction : IBizAction<PlaceOrderInDto, Order> { };
}
