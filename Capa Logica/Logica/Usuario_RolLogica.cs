using Capa_Datos;
using Capa_Entidades;

namespace Capa_Logica;

public class Usuario_RolLogica : LogicaBase<Usuario_Rol, Usuario_RolData>
{
    public Usuario_RolLogica(Usuario_RolData datos) : base(datos) { }
}
