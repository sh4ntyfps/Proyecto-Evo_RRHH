using Capa_Entidades;
using Microsoft.AspNetCore.Identity;

namespace Proyecto_Evo_RRLL.Servicios;

/// <summary>
/// Unica responsabilidad: generar y verificar el hash de las claves de acceso (SRP).
/// Antes el algoritmo (<see cref="PasswordHasher{TUser}"/>) aparecia suelto en el login,
/// en el mantenimiento de usuarios y en la siembra del administrador; centralizarlo
/// evita esa triple copia (DRY) y permite cambiarlo en un solo lugar.
/// </summary>
public class ServicioClaves
{
    private readonly PasswordHasher<Usuario> _algoritmo = new();

    public string Generar(string clave) => _algoritmo.HashPassword(new Usuario(), clave);

    public bool EsValida(string hash, string clave) =>
        _algoritmo.VerifyHashedPassword(new Usuario(), hash, clave) != PasswordVerificationResult.Failed;
}
