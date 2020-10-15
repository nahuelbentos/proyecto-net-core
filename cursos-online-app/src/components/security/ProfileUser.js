import React from 'react';
import style from '../tools/style';
import { Button, Container, Grid, TextField, Typography } from '@material-ui/core';
const ProfileUser = () => {
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
              <TextField name="nombre-completo" variant="outlined" fullWidth label="Ingrese nombre y apellido" />
            </Grid>

            <Grid item xs={12} md={6}>
              <TextField name="email" variant="outlined" fullWidth label="Ingrese Email" />
            </Grid>

            <Grid item xs={12} md={6}>
              <TextField name="password" type="password" variant="outlined" fullWidth label="Ingrese password" />
            </Grid>

            <Grid item xs={12} md={6}>
              <TextField name="confirm-password" type="password" variant="outlined" fullWidth label="Confirme password" />
            </Grid>
          </Grid>

          <Grid container justify="center">
            <Grid item xs={12} md={6}>
              <Button type="submit" fullWidth variant="contained" size="large" color="primary" style={style.submit}>
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
