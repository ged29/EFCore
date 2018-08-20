using BizLogic.GenericInterfaces;
using DataLayer.Entities;

namespace BizLogic.Orders
{
    public interface IPlaceOrderPart2 : IBizAction<Part1ToPart2Dto, Order> { }
}