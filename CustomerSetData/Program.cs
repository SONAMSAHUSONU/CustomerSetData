using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomerSetData;

namespace CustomerSetData
{
    public class Menu
    {
        public string Name;
        public long Mobile;
        public string Address;
        public string Order;

        public static void Main(string[] args)
        {

            Console.WriteLine("Welcome the Restorent");
            Console.WriteLine("1" + "Vag");
            Console.WriteLine("2" + "Non-Vag");
            Console.WriteLine("Enter  1 for VagFood and 2 for Non-Vag ");
            int i = Convert.ToInt32(Console.ReadLine());

            Menu objMenu = new Menu();
            List<OrderDetails> Show = objMenu.GetOrder(i);
            foreach (var item in Show)
            {
                Console.WriteLine(item.id);
                Console.WriteLine(item.orderName);
                Console.WriteLine(item.orderPrice);
                Console.WriteLine(item.orderRateing);
            }
            Console.WriteLine("Please enter your Name");
            objMenu.Name = Console.ReadLine();
            Console.WriteLine("Please enter your Mobile Number");
            objMenu.Mobile = Convert.ToInt64(Console.ReadLine());
            Console.WriteLine("Please enter your Address");
            objMenu.Address = Console.ReadLine();
            Console.WriteLine("Please enter your OrderName");
            objMenu.Order = Console.ReadLine();
            objMenu.ClientDetails(objMenu);

            Console.ReadLine();

        }
        public List<OrderDetails> ClientDetails(Menu objMenu)
        {

            string scn = ConfigurationManager.ConnectionStrings["scn"].ConnectionString;



            using (SqlConnection cn = new SqlConnection(scn))
            {
                cn.Open();
                List<OrderDetails> details = new List<OrderDetails>();


                using (SqlCommand cmd = new SqlCommand("SPCustomer2", cn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CName", objMenu.Name);
                    cmd.Parameters.AddWithValue("@CAddress", objMenu.Address);
                    cmd.Parameters.AddWithValue("@CMobile", objMenu.Mobile);
                    cmd.Parameters.AddWithValue("@COrder", objMenu.Order);

                    int status = cmd.ExecuteNonQuery();

                    if (status > 0)
                    {
                        Console.WriteLine("Your order is Successful");
                    }
                    else
                    {
                        Console.WriteLine("Your order is Failed");

                    }
                }
                return details;

            }
        }


            public List<OrderDetails> GetOrder(int i)
        {
            string scn = ConfigurationManager.ConnectionStrings["scn"].ConnectionString;



            using (SqlConnection cn = new SqlConnection(scn))
            {

                try
                {
                    cn.Open();
                    List<OrderDetails> orders = new List<OrderDetails>();


                    if (i == 1)
                    {
                        using (SqlCommand cmd = new SqlCommand("sp_vegmenu", cn))
                        {
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;

                            SqlDataReader dr = cmd.ExecuteReader();


                            while (dr.Read())
                            {
                                OrderDetails orderDetails = new OrderDetails();
                                orderDetails.id = Convert.ToInt32(dr["v_id"]);
                                orderDetails.orderName = Convert.ToString(dr["v_Name"]);
                                orderDetails.orderPrice = Convert.ToString(dr["v_Price"]);
                                orderDetails.orderRateing = Convert.ToString(dr["v_Rateing"]);
                                orders.Add(orderDetails);

                            }

                        }
                    }
                    else
                    {

                        using (SqlCommand cmd = new SqlCommand("sp_nonvegmenu", cn))
                        {
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            SqlDataReader dr = cmd.ExecuteReader();


                            while (dr.Read())
                            {
                                OrderDetails orderDetails = new OrderDetails();
                                orderDetails.id = Convert.ToInt32(dr["nv_id"]);
                                orderDetails.orderName = Convert.ToString(dr["nv_Name"]);
                                orderDetails.orderPrice = Convert.ToString(dr["nv_Price"]);
                                orderDetails.orderRateing = Convert.ToString(dr["nv_Rateing"]);
                                orders.Add(orderDetails);

                            }
                        }

                    }

                    return orders;

                }

                catch (Exception ex)
                {
                    return null;
                }


                finally
                {
                    cn.Close();
                }

            }
        }
        public class OrderDetails
        {
            public int id { get; set; }
            public string orderName { get; set; }
            public string orderPrice { get; set; }
            public string orderRateing { get; set; }

        }
    }
}




