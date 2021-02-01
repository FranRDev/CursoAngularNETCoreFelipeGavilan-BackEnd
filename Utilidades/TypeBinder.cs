using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace back_end.Utilidades {

    public class TypeBinder<T> : IModelBinder {

        public Task BindModelAsync(ModelBindingContext modelBindingContext) {
            var nombrePropiedad = modelBindingContext.ModelName;
            var valor = modelBindingContext.ValueProvider.GetValue(nombrePropiedad);

            if (valor == ValueProviderResult.None) { return Task.CompletedTask; }

            try {
                var valorDeserializado = JsonSerializer.Deserialize<T>(valor.FirstValue);
                modelBindingContext.Result = ModelBindingResult.Success(valorDeserializado);

            } catch (Exception) {
                modelBindingContext.ModelState.TryAddModelError(nombrePropiedad, "El valor no es del tipo correcto");
            }

            return Task.CompletedTask;
        }

    }

}