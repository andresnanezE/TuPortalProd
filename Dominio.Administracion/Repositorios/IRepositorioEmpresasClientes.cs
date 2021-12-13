using Dominio.Administracion.Entidades;

namespace Dominio.Administracion.Repositorios
{
    public interface IRepositorioEmpresasClientes
    {
        EMSP_EmpresasCliente ObtenerNombreXNIT(decimal NIT);

        string ObtenerNombreClienteXNIT(decimal auxNIT);
    }
}