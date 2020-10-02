using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomerAppApiSql.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CustomerAppApiSql.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        // GET: api/Customer
        [HttpGet]
        public IEnumerable<Customer> Get()
        {
            List<Customer> list = new List<Customer>();
            var where = "";
            var connection = new SqlConnection("Data Source=(local);Initial Catalog=CustomerDB;Integrated Security=True");

            var command = new SqlCommand()
            {
                Connection = connection,
                CommandText = @"SELECT [id],[name],[country],[phone] FROM [CustomerDB].[dbo].[management_customer] " + where
            };
            try
            {
                connection.Open();
                var reader = command.ExecuteReader();
                while(reader.Read()){
                    var customer = new Customer();
                    customer.id = (dynamic)reader["id"];
                    customer.name = reader["name"].ToString();
                    customer.country = reader["country"].ToString();
                    customer.phone = (dynamic)reader["phone"];
                    list.Add(customer);
                }
                reader.Close();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
                connection.Dispose();
                command.Dispose();
            }
            return list;
        }

        // POST : api/Customer
        [HttpPost]
        public void Post(Customer customer)
        {
            var connection = new SqlConnection("Data Source=(local);Initial Catalog=CustomerDB;Integrated Security=True");
            var command = new SqlCommand()
            {
                Connection = connection,
                CommandText = @"INSERT INTO [CustomerDB].[dbo].[management_customer] ([name],[country],[phone]) VALUES (@NAME,@COUNTRY,@PHONE) "

            };
            command.Parameters.Add(new SqlParameter("@NAME", SqlDbType.VarChar));
            command.Parameters["@NAME"].Value = customer.name;
            command.Parameters.Add(new SqlParameter("@COUNTRY", SqlDbType.VarChar));
            command.Parameters["@COUNTRY"].Value = customer.country;
            command.Parameters.Add(new SqlParameter("@PHONE", SqlDbType.Int));
            command.Parameters["@PHONE"].Value = customer.phone;

            try
            {
                connection.Open();
                command.ExecuteNonQuery();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
                connection.Dispose();
                command.Dispose();
            }
        }

        // GET : api/Customer/1
        [HttpGet("{id}")]
        public Customer Get(int id)
        {
            var connection = new SqlConnection("Data Source=(local);Initial Catalog=CustomerDB;Integrated Security=True");
            var command = new SqlCommand()
            {
                 Connection = connection,
                 CommandText = @"SELECT [id],[name],[country],[phone] FROM [CustomerDB].[dbo].[management_customer] where [id] = @ID"
            };
            command.Parameters.Add(new SqlParameter("@ID",SqlDbType.Int));
            command.Parameters["@ID"].Value = id;
            var customer = new Customer();
            try
            {
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    customer.id = (dynamic)reader["id"];
                    customer.name = reader["name"].ToString();
                    customer.country = reader["country"].ToString();
                    customer.phone = (dynamic)reader["phone"];
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
                connection.Dispose();
                command.Dispose();
            }
            return customer;
        }

        // PUT : api/Customer
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Customer customer)
        {
            var connection = new SqlConnection("Data Source=(local);Initial Catalog=CustomerDB;Integrated Security=True");
            var command = new SqlCommand()
            {
                Connection = connection,
                CommandText = @"UPDATE [CustomerDB].[dbo].[management_customer] SET [name] = @NAME,
                                                                            [country] = @COUNTRY,
                                                                            [phone] = @PHONE
                                                                        WHERE [id] = @ID"
            };
            command.Parameters.Add(new SqlParameter("@ID", SqlDbType.VarChar));
            command.Parameters["@ID"].Value = customer.id;
            command.Parameters.Add(new SqlParameter("@NAME", SqlDbType.VarChar));
            command.Parameters["@NAME"].Value = customer.name;
            command.Parameters.Add(new SqlParameter("@COUNTRY", SqlDbType.VarChar));
            command.Parameters["@COUNTRY"].Value = customer.country;
            command.Parameters.Add(new SqlParameter("@PHONE", SqlDbType.VarChar));
            command.Parameters["@PHONE"].Value = customer.phone;

            try
            {
                connection.Open();
                command.ExecuteNonQuery();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
                connection.Dispose();
                command.Dispose();
            }
        }

        // DELETE: api/Customer
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var connection = new SqlConnection("Data Source=(local);Initial Catalog=CustomerDB;Integrated Security=True");
            var command = new SqlCommand()
            {
                Connection = connection,
                CommandText = @"DELETE FROM [CustomerDB].[dbo].[management_customer] where [id] = @id"
            };
            command.Parameters.Add(new SqlParameter("@ID",SqlDbType.Int));
            command.Parameters["@ID"].Value = id;
            try
            {
                connection.Open();
                command.ExecuteNonQuery();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
                connection.Dispose();
                command.Dispose();
            }
        }

        // GET : api/Customer/Name/Alex
        [HttpGet("[action]/{option}/{value}")]
        public IEnumerable<Customer> SearchCustomer(string option, string value)
        {
            List<Customer> list = new List<Customer>();
            var where = new StringBuilder();
            where.Append("WHERE [id] like '%%' ");
            if (option == "name")
            {
                where.Append("AND [name] like '%'+ @VALUE +'%'");
            }
            if (option == "country")
            {
                where.Append("AND [country] like '%'+ @VALUE +'%'");
            }
            var connection = new SqlConnection("Data Source=(local);Initial Catalog=CustomerDB;Integrated Security=True");
            var command = new SqlCommand()
            {
                Connection = connection,
                CommandText = @"SELECT [id],[name],[country],[phone] FROM [CustomerDB].[dbo].[management_customer] 
                                                                      " + where
            };
            command.Parameters.Add(new SqlParameter("@VALUE", SqlDbType.VarChar));
            command.Parameters["@VALUE"].Value = value;

            try
            {
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var customer = new Customer();
                    customer.id = (dynamic)reader["id"];
                    customer.name = reader["name"].ToString();
                    customer.country = reader["country"].ToString();
                    customer.phone = (dynamic)reader["phone"];
                    list.Add(customer);
                }
                reader.Close();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
                connection.Dispose();
                command.Dispose();
            }
            return list;
        }

        // GET : api/Customer/Id/1
        [HttpGet("[action]/{option}/{value}")]
        public IEnumerable<Customer> SortCustomer(string option, int value)
        {
            List<Customer> list = new List<Customer>();
            var orderBy = new StringBuilder();
            if (option == "id")
            {
                if (value == 1)
                {
                    orderBy.Append("id ASC");
                }
                else
                {
                    orderBy.Append("id DESC");

                }
            }
            if (option == "name")
            {
                if (value == 1)
                {
                    orderBy.Append("name ASC");
                }
                else
                {
                    orderBy.Append("name DESC");

                }
            }
            if (option == "country")
            {
                if (value == 1)
                {
                    orderBy.Append("country ASC");
                }
                else
                {
                    orderBy.Append("country DESC");

                }
            }
            var connection = new SqlConnection("Data Source=(local);Initial Catalog=CustomerDB;Integrated Security=True");
            var command = new SqlCommand()
            {
                Connection = connection,
                CommandText = @"SELECT [id],[name],[country],[phone] FROM [CustomerDB].[dbo].[management_customer]  ORDER BY " + orderBy
            };

            try
            {
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var customer = new Customer();
                    customer.id = (dynamic)reader["id"];
                    customer.name = reader["name"].ToString();
                    customer.country = reader["country"].ToString();
                    customer.phone = (dynamic)reader["phone"];
                    list.Add(customer);
                }
                reader.Close();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
                connection.Dispose();
                command.Dispose();
            }
            return list;
        }

    }
}