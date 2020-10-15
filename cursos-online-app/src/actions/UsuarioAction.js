import httpClient from '../services/HttpClient';

export const registrarUsuario = (usuario) => {
  return new Promise((resolve, reject) => {
    httpClient
      .post('/usuario/registrar', usuario)
      .then((response) => {
        resolve(response);
      })
      .catch((error) => reject(error));
  });
};
