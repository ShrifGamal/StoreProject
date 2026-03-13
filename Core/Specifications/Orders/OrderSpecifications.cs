using Store.Core.Entites.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Specifications.Orders
{
    public class OrderSpecifications : BaseSpecifications<Order , int>
    {
        public OrderSpecifications(string buyerId , int OrderId):base(
            O => O.BuyerEmail == buyerId 
            && O.Id == OrderId
            )
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.Items);

        }

        public OrderSpecifications(string buyerId ):base(
            O => O.BuyerEmail == buyerId 
            )
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.Items);

        }
    }
}
