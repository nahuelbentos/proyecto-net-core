import { Grid, MuiThemeProvider, Snackbar } from '@material-ui/core';
import React, { useEffect, useState } from 'react';
import Login from './components/security/Login';
import ProfileUser from './components/security/ProfileUser';
import RegisterUser from './components/security/RegisterUser';
import theme from './theme/theme';
import { Route, BrowserRouter as Router, Switch } from 'react-router-dom';

import { useStateValue } from './context/store';
import { obtenerUsuarioActual } from './actions/UsuarioAction';
import AppNavbar from './components/navigation/AppNavbar';
import SecureRoute from './components/navigation/SecureRoute';
import NuevoCurso from './components/cursos/NuevoCurso';
import PaginadorCurso from './components/cursos/PaginadorCurso';

function App() {
  const [{ sesionUsuario, openSnackbar }, dispatch] = useStateValue();

  const [iniciaApp, setIniciaApp] = useState(false);

  useEffect(() => {
    if (!iniciaApp) {
      obtenerUsuarioActual(dispatch)
        .then((response) => setIniciaApp(true))
        .catch((error) => setIniciaApp(true));
    }
  }, [iniciaApp]);

  return !iniciaApp ? null : (
    <React.Fragment>
      <Snackbar
        anchorOrigin={{ vertical: 'bottom', horizontal: 'center' }}
        open={openSnackbar ? openSnackbar.open : false}
        autoHideDuration={3000}
        ContentProps={{ 'arial-describedby': 'message-id' }}
        message={<span id="message-id"> {openSnackbar ? openSnackbar.mensaje : ''} </span>}
        onClose={() =>
          dispatch({
            type: 'OPEN_SNACKBAR',
            openMensaje: {
              open: false,
              mensaje: '',
            },
          })
        }
      ></Snackbar>
      <Router>
        <MuiThemeProvider theme={theme}>
          <AppNavbar />
          <Grid container>
            <Switch>
              <Route exact path="/auth/login" component={Login} />
              <Route exact path="/auth/register" component={RegisterUser} />

              <SecureRoute exact path="/auth/profile" component={ProfileUser} />
              <SecureRoute exact path="/curso/nuevo" component={NuevoCurso} />
              <SecureRoute exact path="/curso/paginador" component={PaginadorCurso} />
              <SecureRoute exact path="" component={ProfileUser} />
            </Switch>
          </Grid>
        </MuiThemeProvider>
      </Router>
    </React.Fragment>
  );
}

export default App;
