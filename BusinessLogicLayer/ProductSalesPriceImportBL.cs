using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class ProductSalesPriceImportBL
    {
        public void UpdateSalesPrice(string prodCode, decimal mrp, decimal markUpMin, decimal markUpPlus, decimal salePrice, decimal minSalePrice, int UserId, decimal DiscountUpTo)
        {
            ProcedureExecute proc;
            try
            {
                using (proc = new ProcedureExecute("prc_updateSalesprice"))
                {
                    proc.AddVarcharPara("@prodCode", 80, prodCode);
                    proc.AddDecimalPara("@mrp", 2, 18, mrp);
                    proc.AddDecimalPara("@markup_min", 2, 5, markUpMin);
                    proc.AddDecimalPara("@markup_plus", 2, 5, markUpPlus);
                    proc.AddDecimalPara("@sale_price", 2, 18, salePrice);
                    proc.AddDecimalPara("@min_saleprice", 2, 18, minSalePrice);
                    proc.AddIntegerPara("@CreateUser", UserId);
                    proc.AddDecimalPara("@DiscountUpTo", 2, 5, DiscountUpTo);
                    int i = proc.RunActionQuery();
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                proc = null;
            }

        }

        public void UpdateSalesPriceimport(string prodCode, decimal mrp, decimal markUpMin, decimal markUpPlus, decimal salePrice, decimal minSalePrice, int UserId)
        {
            ProcedureExecute proc;
            try
            {
                using (proc = new ProcedureExecute("prc_updateSalespriceimport"))
                {
                    proc.AddVarcharPara("@prodCode", 80, prodCode);
                    proc.AddDecimalPara("@mrp", 2, 18, mrp);
                    proc.AddDecimalPara("@markup_min", 2, 5, markUpMin);
                    proc.AddDecimalPara("@markup_plus", 2, 5, markUpPlus);
                    proc.AddDecimalPara("@sale_price", 2, 18, salePrice);
                    proc.AddDecimalPara("@min_saleprice", 2, 18, minSalePrice);
                    proc.AddIntegerPara("@CreateUser", UserId);                   
                    int i = proc.RunActionQuery();
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                proc = null;
            }

        }

        public void UpdateProductFieldValue(string listProdCode, decimal newPrice, string Updatedfield, int UserId)
        {
            ProcedureExecute proc;
            try
            {
                using (proc = new ProcedureExecute("prc_updateProductField"))
                {
                    proc.AddVarcharPara("@prodCodeList", 1000, listProdCode);
                    proc.AddVarcharPara("@fieldName", 100, Updatedfield);
                    if (Updatedfield == "MRP" || Updatedfield == "SALEP" || Updatedfield == "MSALEP")
                        proc.AddDecimalPara("@newprice", 2, 18, newPrice);
                    else
                        proc.AddDecimalPara("@newMarkup", 2, 5, newPrice);
                    proc.AddIntegerPara("@userId", UserId);

                    int i = proc.RunActionQuery();
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                proc = null;
            }
        }

    }
}
