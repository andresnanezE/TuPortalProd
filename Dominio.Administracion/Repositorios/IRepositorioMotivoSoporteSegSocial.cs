using Dominio.Administracion.Entidades;
using System.Collections.Generic;

namespace Dominio.Administracion.Repositorios
{
    public interface IRepositorioMotivoSoporteSegSocial
    {
        IEnumerable<MotivoSoporteSegSocial> MotivosSoporteSegSocial();

        void AgregarLog(MotivoSoporteSegSocial mss);
    }
}