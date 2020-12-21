# TraductorRecursos

Este programa traduce recursos mediante la API de DeepL.

Requiere que se pase por parámetro un archivo .INI con los recursos a traducir del inglés a otro idioma.
Requiere además una clave de autenticación de API obtenida al comprar un [plan para desarrolladores de DeepL](https://www.deepl.com/pro#developer).
Requiere además que indiquemos el idioma al que traducir como IT para italiano.

El programa carga el archivo .INI e imprime por la consola estándar el archivo traducido, de manera que para obtener el resultado tendremos que realizar una redirección como la siguiente:

```cmd
TraductorRecursos.exe recursos_a_traducir_al_italiano.ini [API] IT >  recursos_en_italiano.ini
```
