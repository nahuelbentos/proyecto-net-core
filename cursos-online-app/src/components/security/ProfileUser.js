import React, { useEffect, useState } from 'react';
import style from '../tools/style';
import { Avatar, Button, Container, Grid, TextField, Typography } from '@material-ui/core';
import { actualizarUsuario, obtenerUsuarioActual } from '../../actions/UsuarioAction';
import { useStateValue } from '../../context/store';
import reactFoto from '../../logo.svg';
import { v4 as uuidv4 } from 'uuid';
import ImageUploader from 'react-images-upload';
import { obtenerDataImagen } from '../../actions/ImagenAction';

const ProfileUser = () => {
  const [{ sesionUsuario }, dispatch] = useStateValue();

  const [usuario, setUsuario] = useState({
    nombreCompleto: '',
    email: '',
    userName: '',
    password: '',
    confirmarPaswword: '',
    imagenPerfil: null,
    fotoUrl: '',
  });

  const ingresarValoresMemoria = (e) => {
    const { name, value } = e.target;
    setUsuario((anterior) => ({
      ...anterior,
      [name]: value,
    }));
  };

  useEffect(() => {
    setUsuario(sesionUsuario.usuario);
    setUsuario((anterior) => ({
      ...anterior,
      fotoUrl: sesionUsuario.usuario.imagenPerfil,
    }));
  }, []);

  const subirFoto = (imagenes) => {
    const foto = imagenes[0];
    const fotoUrl = URL.createObjectURL(foto);

    console.log(foto);
    obtenerDataImagen(foto).then((imagenPerfil) =>
      setUsuario((anterior) => ({
        ...anterior,
        imagenPerfil, // el archivo en Formato json { data, nombre, extension}
        fotoUrl, // el archivo en formato url
      }))
    );
  };

  const fotoKey = uuidv4();

  const guardarCambios = (element) => {
    element.preventDefault();
    actualizarUsuario(usuario, dispatch)
      .then((response) => {
        if (response.status === 200) {
          dispatch({
            type: 'OPEN_SNACKBAR',
            openMensaje: {
              open: true,
              mensaje: 'Se guardaron exitosamente los cambios en Perfil Usuario',
            },
          });
        }

        localStorage.setItem('token_seguridad', response.data.token);
      })
      .catch((error) => {
        console.log('error: ', error);
        dispatch({
          type: 'OPEN_SNACKBAR',
          openMensaje: {
            open: true,
            mensaje: 'Errores al intentar guardar en : ' + Object.keys(error.data.errors),
          },
        });
      });
  };

  return (
    <Container component="main" maxWidth="md" justify="center">
      <div style={style.paper}>
        <Avatar style={style.avatar} src={usuario.fotoUrl || reactFoto}></Avatar>

        <Typography component="h1" variant="h5">
          Perfil de Usuario
        </Typography>

        <form style={style.form}>
          <Grid container spacing={2}>
            <Grid item xs={12} md={12}>
              <TextField
                name="nombreCompleto"
                variant="outlined"
                fullWidth
                label="Ingrese nombre y apellido"
                value={usuario.nombreCompleto}
                onChange={ingresarValoresMemoria}
              />
            </Grid>

            <Grid item xs={12} md={6}>
              <TextField
                name="email"
                value={usuario.email}
                variant="outlined"
                fullWidth
                label="Ingrese Email"
                onChange={ingresarValoresMemoria}
              />
            </Grid>

            <Grid item xs={12} md={6}>
              <TextField
                name="userName"
                value={usuario.userName}
                variant="outlined"
                fullWidth
                label="Ingrese Username"
                onChange={ingresarValoresMemoria}
              />
            </Grid>

            <Grid item xs={12} md={6}>
              <TextField
                name="password"
                type="password"
                variant="outlined"
                fullWidth
                label="Ingrese password"
                value={usuario.password}
                onChange={ingresarValoresMemoria}
              />
            </Grid>

            <Grid item xs={12} md={6}>
              <TextField
                name="confirmarPaswword"
                type="password"
                variant="outlined"
                fullWidth
                label="Confirme password"
                value={usuario.confirmarPaswword}
                onChange={ingresarValoresMemoria}
              />
            </Grid>

            <Grid item xs={12} md={12}>
              <ImageUploader
                withIcon={false}
                key={fotoKey}
                singleImage={true}
                buttonText="Seleccione una imagen de perfil"
                onChange={subirFoto}
                imgExtension={['.jpg', '.gif', '.png', '.jpeg']}
                maxFileSize={5242880}
              />
            </Grid>
          </Grid>
          <Grid container justify="center">
            <Grid item xs={12} md={6}>
              <Button
                type="submit"
                fullWidth
                variant="contained"
                size="large"
                color="primary"
                style={style.submit}
                onClick={guardarCambios}
              >
                Guardar Datos
              </Button>
            </Grid>
          </Grid>
        </form>
      </div>
    </Container>
  );
};

export default ProfileUser;
