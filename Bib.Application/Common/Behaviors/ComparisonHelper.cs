using System.Reflection;

namespace Bib.Application.Common.Behaviors
{
    public static class ComparisonHelper
    {
        public static bool Changed<T>(T existente, T atualizado)
        {
            var propriedades = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in propriedades)
            {
                if (prop.Name == "Id" || prop.Name == "CreatedAt")
                    continue;

                var valorExistente = prop.GetValue(existente);
                var valorAtualizado = prop.GetValue(atualizado);

                if (!Equals(valorExistente, valorAtualizado))
                    return true;
            }

            return false;
        }
    }
}
