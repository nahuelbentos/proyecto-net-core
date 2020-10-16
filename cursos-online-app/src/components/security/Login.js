import React, { useState } from 'react';
import { Avatar, Button, Container, TextField, Typography } from '@material-ui/core';
import LockOutlinedIcon from '@material-ui/icons/LockOutlined';

import style from '../tools/style';
import { loginUsuario } from '../../actions/UsuarioAction';

const Login = () => {
  const [usuario, setUsuario] = useState({
    email: '',
    password: '',
  });

  const ingresarValoresMemoria = (e) => {
    const { name, value } = e.target;
    setUsuario((anterior) => ({
      ...anterior,
      [name]: value,
    }));
  };

  const login = (e) => {
    e.preventDefault();
    console.log('voy a loguear el problema');
    loginUsuario(usuario)
      .then((response) => {
        console.log('Se logueo el usuario exitosamente', response);
        localStorage.setItem('token_seguridad', response.data.token);
      })
      .catch((error) => console.log(error));
    // loginUsuario(usuario).then((response) => localStorage.setItem('token_seguridad', response.data.token));
  };

  return (
    <Container maxWidth="xs">
      <div style={style.paper}>
        <Avatar style={style.avatar}>
          <LockOutlinedIcon style={style.icon} />
        </Avatar>
        <Typography component="h1" variant="h5">
          Login de Usuario
        </Typography>
        <form style={style.form}>
          <TextField
            variant="outlined"
            label="Ingrese username"
            name="email"
            fullWidth
            onChange={ingresarValoresMemoria}
            value={usuario.email}
            margin="normal"
          />
          <TextField
            variant="outlined"
            label="Ingrese password"
            type="password"
            name="password"
            fullWidth
            onChange={ingresarValoresMemoria}
            value={usuario.password}
            margin="normal"
          />
          <Button type="submit" fullWidth variant="contained" color="primary" style={style.submit} onClick={login}>
            Ingresar
          </Button>
        </form>
      </div>
    </Container>
  );
};

export default Login;
