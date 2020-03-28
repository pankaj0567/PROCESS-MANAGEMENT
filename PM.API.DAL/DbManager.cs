using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;


namespace PM.API.DAL
{
    public class DbManager : IDbManager
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataReader dr;
        private readonly string constr;

        public DbManager(IWebConfiguration Constr)
        {
            constr = Constr.DefaultConnection;
        }
        //public IEnumerable<SKUPriceDetails> GetSKUPriceDetailsById(string inParams)
        //{
        //    con = new SqlConnection(constr);
        //    con.Open();
        //    cmd = new SqlCommand("[Feed].[B2C_GetSKUDetailsBySKUID]", con)
        //    {
        //        CommandType = CommandType.StoredProcedure
        //    };
        //    cmd.Parameters.AddWithValue("@inputParam", inParams);
        //    dr = cmd.ExecuteReader();
        //    var sk = new List<SKUPriceDetails>();
        //    while (dr.Read())
        //    {
        //        SKUPriceDetails sKUPriceDetails = new SKUPriceDetails
        //        {
        //            SkuId = Convert.ToInt32(dr["skuId"]),
        //            ListPrice = Convert.ToInt32(dr["listPrice"]),
        //            MrpPrice = Convert.ToInt32(dr["mrpPrice"]),
        //            TaxDesc = dr["taxDesc"].ToString(),
        //        };
        //        sk.Add(sKUPriceDetails);
        //    }
        //    return sk;
        //}

        //public IEnumerable<OrderDetails> GetOrderDetails(string inParams)
        //{
        //    con = new SqlConnection(constr);
        //    con.Open();
        //    cmd = new SqlCommand("[Feed].[B2C_GetOrderDetails]", con)
        //    {
        //        CommandType = CommandType.StoredProcedure
        //    };
        //    cmd.Parameters.AddWithValue("@inputParam", inParams);
        //    dr = cmd.ExecuteReader();
        //    var sk = new List<OrderDetails>();
        //    var orderList = new List<Order>();
        //    while (dr.Read())
        //    {
        //        var obj = new Order
        //        {
        //            OrderNumber = Convert.ToInt32(dr["orderNumber"]),
        //            StoreId = Convert.ToString(dr["storeId"]),
        //            Status = dr["status"].ToString(),
        //            Quantity = dr["quantity"].ToString(),
        //            OrderDate = Convert.ToDateTime(dr["orderDate"]),
        //            ShippingDate = Convert.ToDateTime(dr["shippingDate"])
        //        };
        //        orderList.Add(obj);
        //    }

        //    var firstResultSet = orderList
        //                            .GroupBy(o => new { o.OrderNumber, o.StoreId, o.OrderDate, o.ShippingDate })
        //                            .Select(o => new { o.Key.OrderNumber, o.Key.StoreId, o.Key.OrderDate, o.Key.ShippingDate, CatalogRefId = o.ToList() });

        //    foreach (var item in firstResultSet)
        //    {
        //        var orderDetails = new OrderDetails
        //        {
        //            OrderNumber = item.OrderNumber,
        //            StoreId = item.StoreId,
        //            OrderDate = item.OrderDate,
        //            ShippingDate = item.ShippingDate
        //        };

        //        var lIs = new List<CatalogRefId>();

        //        foreach (var cat in item.CatalogRefId)
        //        {
        //            var catalog = new CatalogRefId
        //            {
        //                Status = cat.Status,
        //                Quantity = cat.Quantity
        //            };
        //            lIs.Add(catalog);
        //        }
        //        orderDetails.LineItems = new List<LineItems> {
        //            new LineItems{
        //                    CatalogRefId =lIs
        //            }
        //        };
        //        sk.Add(orderDetails);
        //    }
        //    return sk;
        //}

        //public IEnumerable<SKUPriceDetails> GetSKUPriceDetailsByDate(string inParams)
        //{
        //    con = new SqlConnection(constr);
        //    con.Open();
        //    cmd = new SqlCommand("[Feed].[B2C_GetSKUDetailsByDate]", con)
        //    {
        //        CommandType = CommandType.StoredProcedure
        //    };
        //    cmd.Parameters.AddWithValue("@inputParam", inParams);
        //    dr = cmd.ExecuteReader();
        //    var sk = new List<SKUPriceDetails>();
        //    while (dr.Read())
        //    {
        //        SKUPriceDetails sKUPriceDetails = new SKUPriceDetails
        //        {
        //            SkuId = Convert.ToInt32(dr["skuId"]),
        //            ListPrice = Convert.ToInt32(dr["listPrice"]),
        //            MrpPrice = Convert.ToInt32(dr["mrpPrice"]),
        //            TaxDesc = dr["taxDesc"].ToString(),
        //        };
        //        sk.Add(sKUPriceDetails);
        //    }
        //    return sk;
        //}

        public void SendOrder(string xml)
        {
            con = new SqlConnection(constr);
            con.Open();
            cmd = new SqlCommand("[Feed].[TestXml]", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            var param = new SqlParameter()
            {
                ParameterName = "@inputXml",
                Value =  xml,
                SqlDbType = SqlDbType.Xml
            };
            cmd.Parameters.Add(param);
            int i = cmd.ExecuteNonQuery();
        }

        public string CreateOrder(string xml)
        {
            con = new SqlConnection(constr);
            con.Open();
            cmd = new SqlCommand("[FEED].[CreateOrder]", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            var param = new SqlParameter()
            {
                ParameterName = "@inputXml",
                Value = xml,
                SqlDbType = SqlDbType.VarChar
            };
            cmd.Parameters.Add(param);
            cmd.Parameters.Add("@Msg", SqlDbType.Char, 500);
            cmd.Parameters["@Msg"].Direction = ParameterDirection.Output;
            cmd.ExecuteNonQuery();
            string msg = (string)cmd.Parameters["@Msg"].Value;
            return msg;
        }
    }
}
