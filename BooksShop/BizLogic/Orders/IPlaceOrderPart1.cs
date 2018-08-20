using BizLogic.GenericInterfaces;

namespace BizLogic.Orders
{
    public interface IPlaceOrderPart1 : IBizAction<PlaceOrderInDto, Part1ToPart2Dto> { }
}