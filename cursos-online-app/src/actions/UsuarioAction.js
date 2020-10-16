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
        if (response.data.imagenPerfil) {
          let imagenPerfil = response.data.imagenPerfil;
          const nuevaFoto = `data:image/${imagenPerfil.extension};base64,${imagenPerfil.data}`;
          response.data.imagenPerfil = nuevaFoto;
        }
        dispatch({
          type: 'INICIAR_SESION',
          sesion: response.data,
          autenticado: true,
        });
        resolve(response);
      })
      .catch((error) => reject(error))
  );

export const actualizarUsuario = (usuario, dispatch) =>
  new Promise((resolve, reject) =>
    httpClient
      .put('/usuario', usuario)
      .then((response) => {
        if (response.data && response.data.imagenPerfil) {
          let imagenPerfil = response.data.imagenPerfil;
          const nuevaFile = `data:image/${imagenPerfil.extension};base64,${imagenPerfil.data}`;
          response.data.imagenPerfil = nuevaFile;
        }

        dispatch({
          type: 'INICIAR_SESION',
          sesion: response.data,
          autenticado: true,
        });

        resolve(response);
      })
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
