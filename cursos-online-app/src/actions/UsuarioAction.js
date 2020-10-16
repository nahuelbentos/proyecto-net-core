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

export const obtenerUsuarioActual = (dispatch) =>
  new Promise((resolve, reject) =>
    httpClient
      .get('/usuario')
      .then((response) => {
        dispatch({
          type: 'INICIAR_SESION',
          sesion: response.data,
          autenticado: true,
        });
        resolve(response);
      })
      .catch((error) => reject(error))
  );

export const actualizarUsuario = (usuario) =>
  new Promise((resolve, reject) =>
    httpClient
      .put('/usuario', usuario)
      .then((response) => resolve(response))
      .catch((error) => reject(error.response))
  );

export const loginUsuario = (usuario) =>
  new Promise(
    (resolve, reject) =>
      httpClient
        .post('/usuario/login', usuario)
        .then((response) => resolve(response))
        .catch((error) => resolve(error.response))
    // .catch((error) => resolve(error.response)) // No se porque no lo resuelve con un reject, pero bue
  );
