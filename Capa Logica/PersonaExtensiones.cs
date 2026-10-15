using Capa_Entidades;

namespace Capa_Logica;

public static class PersonaExtensiones
{
    // Unico lugar donde se arma el nombre completo de una persona (DRY).
    public static string NombreCompleto(this Persona persona)
        => $"{persona.Nombres} {persona.Apellido_Paterno} {persona.Apellido_Materno}".Trim();
}
