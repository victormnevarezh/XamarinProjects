﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiUberEats.Models
{
    public class OrdenesModel
    {
        public string Fecha { get; set; }

        public string Cliente { get; set; }

        public double Total { get; set; }

        public int IdOrden { get; set; }

        public int IdRestaurante { get; set; }

        public List<OrdenesModel> GetAll(string ConnectionString, int id)
        {
            List<OrdenesModel> ordenes = new List<OrdenesModel>();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    string tsql = "SELECT * FROM Ordenes Where IdRestaurante = @Id";
                    using (SqlCommand cmd = new SqlCommand(tsql, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ordenes.Add(new OrdenesModel
                                {
                                    IdOrden= (int)reader["IdOrden"],
                                    Cliente = reader["Cliente"].ToString(),
                                    Fecha = reader["Fecha"].ToString(),
                                    Total = (double)reader["Total"],
                                    IdRestaurante = (int)reader["IdRestaurante"]
                                });
                            }
                        }
                    }
                }
                return ordenes;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ResponseModel Insert(string ConnectionString)
        {
            try
            {
                object newId;
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    string tsql = "Insert into Ordenes (Cliente, Fecha, Total, IdRestaurante) Values (@Cliente, @Fecha, @Total, @IdRestaurante) SELECT SCOPE_IDENTITY();";
                    using (SqlCommand cmd = new SqlCommand(tsql, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.Parameters.AddWithValue("@Cliente", this.Cliente);
                        cmd.Parameters.AddWithValue("@Fecha", this.Fecha);
                        cmd.Parameters.AddWithValue("@Total", this.Total);
                        cmd.Parameters.AddWithValue("@IdRestaurante", this.IdRestaurante);
                        newId = cmd.ExecuteScalar();
                        if (newId != null && newId.ToString().Length > 0)
                        {
                            return new ResponseModel
                            {
                                IsSucces = true,
                                Message = "Orden registrada con exito",
                                Response = newId
                            };
                        }
                        else
                        {
                            return new ResponseModel
                            {
                                IsSucces = false,
                                Message = "Error al registrar la orden",
                                Response = null
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    IsSucces = false,
                    Message = $"Error al registrar la orden ({ex})",
                    Response = null
                };
            }
        }

    }
}
