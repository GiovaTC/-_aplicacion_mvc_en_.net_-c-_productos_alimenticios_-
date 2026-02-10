using ProductosAlimenticiosMVC.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ProductosAlimenticiosMVC.Data
{
    public class ProductoDAO
    {
        string cadena =
        "User Id=system;" +
        "Password=Tapiero123;" +
        "Data Source=(DESCRIPTION=" +
        "(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521))" +
        "(CONNECT_DATA=(SERVICE_NAME=orcl))" +
        ");";   

        public List<Producto> Listar()
        {
            List<Producto> lista = new List<Producto>();

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SP_PRODUCTO_CRUD", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@OPCION", 1);

                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    lista.Add(new Producto
                    {
                        IdProducto = (int)dr["ID_PRODUCTO"],
                        Nombre = dr["NOMBRE"].ToString(),
                        Categoria = dr["CATEGORIA"].ToString(),
                        Precio = (decimal)dr["PRECIO"],
                        Stock = (int)dr["STOCK"],
                        FechaVencimiento = (DateTime)dr["FECHA_VENCIMIENTO"]
                    });
                }
            }
            return lista;
        }

        public void EjecutarSP(int opcion, Producto p)
        {
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SP_PRODUCTO_CRUD", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@OPCION", opcion);
                cmd.Parameters.AddWithValue("@ID_PRODUCTO", p.IdProducto);
                cmd.Parameters.AddWithValue("@NOMBRE", p.Nombre);
                cmd.Parameters.AddWithValue("@CATEGORIA", p.Categoria);
                cmd.Parameters.AddWithValue("@PRECIO", p.Precio);
                cmd.Parameters.AddWithValue("@STOCK", p.Stock);
                cmd.Parameters.AddWithValue("@FECHA_VENCIMIENTO", p.FechaVencimiento);

                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}