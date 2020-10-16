import React, { useState } from 'react';
import { Avatar, Button, Container, TextField, Typography } from '@material-ui/core';
import LockOutlinedIcon from '@material-ui/icons/LockOutlined';

import style from '../tools/style';
import { loginUsuario } from '../../actions/UsuarioAction';
import { withRouter } from 'react-router-dom';
import { useStateValue } from '../../context/store';

const Login = (props) => {
  const [{ sesionUsuario }, dispatch] = useStateValue();

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
    loginUsuario(usuario, dispatch)
      .then((response) => {
        localStorage.setItem('token_seguridad', response.data.token);
        props.history.push('/');
      })
      .catch((error) => {
        console.log('el error es: ', error);
        dispatch({
          type: 'OPEN_SNACKBAR',
          openMensaje: {
            open: true,
            mensaje: 'Las credenciales del usuario son incorrectas',
          },
        });
      });
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

export default withRouter(Login);
