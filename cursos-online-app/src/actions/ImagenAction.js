export const obtenerDataImagen = (imagen) =>
  new Promise((resolve, reject) => {
    const nombre = imagen.name;
    const extension = imagen.name.split('.').pop();

    const reader = new FileReader();
    reader.readAsDataURL(imagen);
    reader.onload = () =>
      resolve({
        data: reader.result.split(',')[1],
        nombre,
        extension,
      });
    reader.onerror = (error) => reject(error);
  });
