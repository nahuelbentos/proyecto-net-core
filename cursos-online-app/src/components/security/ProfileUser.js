import React, { useEffect, useState } from 'react';
import style from '../tools/style';
import { Button, Container, Grid, TextField, Typography } from '@material-ui/core';
import { actualizarUsuario, obtenerUsuarioActual } from '../../actions/UsuarioAction';
import { useStateValue } from '../../context/store';
const ProfileUser = () => {
  const [{ sesionUsuario }, dispatch] = useStateValue();

  const [usuario, setUsuario] = useState({
    nombreCompleto: '',
    email: '',
    userName: '',
    password: '',
    confirmarPaswword: '',
  });

  const ingresarValoresMemoria = (e) => {
    const { name, value } = e.target;
    setUsuario((anterior) => ({
      ...anterior,
      [name]: value,
    }));
  };

  const guardarCambios = (element) => {
    element.preventDefault();
    actualizarUsuario(usuario).then((response) => localStorage.setItem('token_seguridad', response.data.token));
  };

  useEffect(() => {
    obtenerUsuarioActual(dispatch).then((response) => setUsuario(response.data));
  }, []);

  return (
    <div>
      <Container component="main" maxWidth="md" justify="center">
        <div style={style.paper}>
          <Typography component="h1" variant="h5">
            Perfil de Usuario
          </Typography>
        </div>
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
      </Container>
    </div>
  );
};

export default ProfileUser;
