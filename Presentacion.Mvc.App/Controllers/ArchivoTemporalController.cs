using System;
using System.Web.Mvc;

namespace Presentacion.Mvc.App.Controllers
{
    public class ArchivoTemporalController : Controller
    {
        [HttpGet]
        public virtual ActionResult Download(string fileGuid, string fileName, string contentType)
        {
            try
            {
                if (TempData[fileGuid] != null)
                {
                    byte[] data = TempData[fileGuid] as byte[];

                    if (contentType.Equals("xls"))
                    {
                        return File(data, "application/vnd.ms-excel", fileName);
                    }
                    else if (contentType.Equals("pdf"))
                    {
                        return File(data, "application/pdf", fileName);
                    }
                    else
                    {
                        return File(data, "application/vnd.ms-excel", fileName);
                    }
                }
                else
                {
                    return new EmptyResult();
                }
            }
            catch (Exception)
            {
                return new EmptyResult();
            }
        }
    }
}