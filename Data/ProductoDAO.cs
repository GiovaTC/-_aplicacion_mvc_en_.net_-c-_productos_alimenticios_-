using ProductosAlimenticiosMVC.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace ProductosAlimenticiosMVC.Data
{
    public class ProductoDAO
    {
        private string cadena =
            "User Id=system;" +
            "Password=Tapiero123;" +
            "Data Source=localhost:1521/orcl;";

        // ===============================
        // LISTAR (OPCION = 1)
        // ===============================
        public List<Producto> Listar()
        {
            List<Producto> lista = new List<Producto>();

            using (OracleConnection cn = new OracleConnection(cadena))
            {
                OracleCommand cmd = new OracleCommand("SP_PRODUCTO_CRUD", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("P_OPCION", OracleDbType.Int32).Value = 1;

                cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor)
                               .Direction = ParameterDirection.Output;

                cn.Open();

                using (OracleDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new Producto
                        {
                            IdProducto = Convert.ToInt32(dr["ID_PRODUCTO"]),
                            Nombre = dr["NOMBRE"].ToString(),
                            Categoria = dr["CATEGORIA"].ToString(),
                            Precio = Convert.ToDecimal(dr["PRECIO"]),
                            Stock = Convert.ToInt32(dr["STOCK"]),
                            FechaVencimiento = Convert.ToDateTime(dr["FECHA_VENCIMIENTO"])
                        });
                    }
                }
            }

            return lista;
        }

        // ===============================
        // INSERTAR / ACTUALIZAR / ELIMINAR
        // ===============================
        public void EjecutarSP(int opcion, Producto p)
        {
            using (OracleConnection cn = new OracleConnection(cadena))
            {
                OracleCommand cmd = new OracleCommand("SP_PRODUCTO_CRUD", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("P_OPCION", OracleDbType.Int32).Value = opcion;
                cmd.Parameters.Add("P_ID_PRODUCTO", OracleDbType.Int32).Value =
                    p.IdProducto == 0 ? (object)DBNull.Value : p.IdProducto;

                cmd.Parameters.Add("P_NOMBRE", OracleDbType.Varchar2).Value = p.Nombre;
                cmd.Parameters.Add("P_CATEGORIA", OracleDbType.Varchar2).Value = p.Categoria;
                cmd.Parameters.Add("P_PRECIO", OracleDbType.Decimal).Value = p.Precio;
                cmd.Parameters.Add("P_STOCK", OracleDbType.Int32).Value = p.Stock;
                cmd.Parameters.Add("P_FECHA_VENCIMIENTO", OracleDbType.Date).Value = p.FechaVencimiento;

                // Cursor obligatorio aunque no se use
                cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor)
                               .Direction = ParameterDirection.Output;

                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
