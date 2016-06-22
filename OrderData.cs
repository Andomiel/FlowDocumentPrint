using System.Collections.Generic;
using System.Linq;

namespace WpfPrintDemo
{
    public class OrderMaster
    {
        public string OrderNo { get; set; }
        public string CustomerName { get; set; }
        public string ShipAddress { get; set; }
        public string Express { get; set; }
        public decimal Freight { get; set; }
        public List<OrderDetail> OrderDetails
        {
            get
            {
                return m_orderDetails;
            }
        }
        public List<OrderDetail> m_orderDetails = new List<OrderDetail>();

        public decimal TotalPrice
        {
            get
            {
                return m_orderDetails.Sum(o => o.UnitPrice * o.Number) + Freight;
            }
        }
    }

    public class OrderDetail
    {
        public string Sku { get; set; }
        public string Spec { get; set; }
        public decimal Number { get; set; }
        public string Unit { get; set; }
        public decimal UnitPrice { get; set; }
        public string Description { get; set; }
    }
}
