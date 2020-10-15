import { Button, Container, Grid, TextField, Typography } from '@material-ui/core';
import React, { useState } from 'react';
import style from '../tools/style';
import { registrarUsuario } from '../../actions/UsuarioAction';

const RegisterUser = () => {
  const [usuario, setusuario] = useState({
    NombreCompleto: '',
    Email: '',
    Password: '',
    ConfirmarPassword: '',
    Username: '',
  });

  const ingresarValoresMemoria = (e) => {
    const { name, value } = e.target;

    setusuario((anterior) => ({
      ...anterior,
      [name]: value,
    }));
  };

  const registrarUsuarioBoton = (e) => {
    e.preventDefault();
    registrarUsuario(usuario).then((response) => {
      console.log('Se registro el usuario exitosamente', response);
      localStorage.setItem('token_seguridad', response.data.token);
    });
  };

  return (
    <Container component="main" maxWidth="md" justify="center">
      <div style={style.paper}>
        <Typography component="h1" variant="h5">
          {' '}
          Registro de Usuario{' '}
        </Typography>

        <form style={style.form}>
          <Grid container spacing={2}>
            <Grid item xs={12} md={12}>
              <TextField
                name="NombreCompleto"
                variant="outlined"
                fullWidth
                label="Ingrese nombre y apellido"
                value={usuario.NombreCompleto}
                onChange={ingresarValoresMemoria}
              />
            </Grid>

            <Grid item xs={12} md={6}>
              <TextField
                name="Email"
                type="email"
                variant="outlined"
                fullWidth
                label="Ingrese su email"
                value={usuario.Email}
                onChange={ingresarValoresMemoria}
              />
            </Grid>

            <Grid item xs={12} md={6}>
              <TextField
                name="Username"
                variant="outlined"
                fullWidth
                label="Ingrese su username"
                value={usuario.Username}
                onChange={ingresarValoresMemoria}
              />
            </Grid>

            <Grid item xs={12} md={6}>
              <TextField
                name="Password"
                type="password"
                variant="outlined"
                fullWidth
                label="Ingrese su password"
                value={usuario.Password}
                onChange={ingresarValoresMemoria}
              />
            </Grid>

            <Grid item xs={12} md={6}>
              <TextField
                name="ConfirmarPassword"
                type="password"
                variant="outlined"
                fullWidth
                label="Confirme de su password"
                value={usuario.ConfirmarPassword}
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
                color="primary"
                size="large"
                style={style.submit}
                onClick={registrarUsuarioBoton}
              >
                Enviar
              </Button>
            </Grid>
          </Grid>
        </form>
      </div>
    </Container>
  );
};

export default RegisterUser;
