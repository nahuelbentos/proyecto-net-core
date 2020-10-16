import httpClient from '../services/HttpClient';
export const guardarCurso = async (curso, imagen) => {
  const endPointCurso = '/cursos';
  const promesaCurso = httpClient.post(endPointCurso, curso);

  if (imagen) {
    const endPointImagen = '/documento';
    const promesaImagen = httpClient.post(endPointImagen, imagen);
    return await Promise.all([promesaCurso, promesaImagen]);
  } else {
    return await Promise.all([promesaCurso]);
  }
};
