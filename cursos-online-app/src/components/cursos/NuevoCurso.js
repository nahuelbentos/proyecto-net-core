import React, { useState } from 'react';
import { Button, Container, Grid, TextField, Typography } from '@material-ui/core';
import style from '../tools/style';
import { KeyboardDatePicker, MuiPickersUtilsProvider } from '@material-ui/pickers';
import DateFnsUtils from '@date-io/date-fns';
import ImageUploader from 'react-images-upload';
import { obtenerDataImagen } from '../../actions/ImagenAction';
import { v4 as uuidv4 } from 'uuid';
import { guardarCurso } from '../../actions/CursoAction';
import { useStateValue } from '../../context/store';

const NuevoCurso = () => {
  const [{ sesionUsuario }, dispatch] = useStateValue();

  const [fechaSeleccionada, setFechaSeleccionada] = useState(new Date());
  const [imagenCurso, setImagenCurso] = useState(null);

  const [curso, setCurso] = useState({
    titulo: '',
    descripcion: '',
    precio: 0.0,
    promocion: 0.0,
  });

  const insgresarValoresMemoria = (e) => {
    const { name, value } = e.target;

    setCurso((anterior) => ({
      ...anterior,
      [name]: value,
    }));
  };

  const subirFoto = (imagenes) => {
    const foto = imagenes[0];

    obtenerDataImagen(foto).then((imagen) => {
      setImagenCurso(imagen);
    });
  };

  const resetearForm = () => {
    setFechaSeleccionada(new Date());
    setImagenCurso(null);
    setCurso({
      titulo: '',
      descripcion: '',
      precio: 0.0,
      promocion: 0.0,
    });
  };

  const fotoKey = uuidv4();

  const guardarCursoEvento = (e) => {
    e.preventDefault();

    const cursoId = uuidv4();
    const objetoCurso = {
      titulo: curso.titulo,
      descripcion: curso.descripcion,
      precio: parseFloat(curso.precio || 0.0),
      promocion: parseFloat(curso.promocion || 0.0),
      fechaPublicacion: fechaSeleccionada,
      cursoId,
    };
    let objetoImagen = null;

    if (imagenCurso) {
      objetoImagen = {
        nombre: imagenCurso.nombre,
        data: imagenCurso.data,
        extension: imagenCurso.extension,
        objetoReferencia: cursoId,
      };
    }

    guardarCurso(objetoCurso, objetoImagen).then((responses) => {
      console.log('respuestas: ', responses);
      const responseCurso = responses[0];
      const responseImagen = responses[1];

      let mensaje = '';

      if (responseCurso.status === 200) {
        mensaje += 'Se guardo exitosamente el curso';
      } else {
        mensaje += 'Errores: ' + Object.keys(responseCurso.data.errores);
      }

      if (responseImagen) {
        if (responseImagen.status === 200) {
          mensaje += ', se guardo la imagen el curso';
        } else {
          mensaje += ', Errores en imagen: ' + Object.keys(responseImagen.data.errores);
        }
      }

      dispatch({
        type: 'OPEN_SNACKBAR',
        openMensaje: {
          open: true,
          mensaje,
        },
      });

      resetearForm();
    });
  };

  return (
    <Container component="main" maxWidth="md" justify="center">
      <div style={style.paper}>
        <Typography component="h1" variant="h5">
          Registro de Nuevo Curso
        </Typography>
        <form style={style.form}>
          <Grid container spacing={2}>
            <Grid item xs={12} md={12}>
              <TextField
                name="titulo"
                variant="outlined"
                fullWidth
                label="Ingrese Titulo"
                value={curso.titulo}
                onChange={insgresarValoresMemoria}
              />
            </Grid>

            <Grid item xs={12} md={12}>
              <TextField
                name="descripcion"
                variant="outlined"
                fullWidth
                label="Ingrese Descripcion"
                value={curso.descripcion}
                onChange={insgresarValoresMemoria}
              />
            </Grid>

            <Grid item xs={12} md={6}>
              <TextField
                name="precio"
                variant="outlined"
                fullWidth
                label="Ingrese Precio Normal"
                value={curso.precio}
                onChange={insgresarValoresMemoria}
              />
            </Grid>

            <Grid item xs={12} md={6}>
              <TextField
                name="promocion"
                variant="outlined"
                fullWidth
                label="Ingrese Precio Promocion"
                value={curso.promocion}
                onChange={insgresarValoresMemoria}
              />
            </Grid>

            <Grid item xs={12} md={6}>
              <MuiPickersUtilsProvider utils={DateFnsUtils}>
                <KeyboardDatePicker
                  value={fechaSeleccionada}
                  onChange={setFechaSeleccionada}
                  margin="normal"
                  id="fecha-pucliacion"
                  label="Seleccione fecha"
                  format="dd/MM/yyyy"
                  fullWidth
                  KeyboardButtonProps={{
                    'arial-label': 'change date',
                  }}
                />
              </MuiPickersUtilsProvider>
            </Grid>

            <Grid item xs={12} md={6}>
              <ImageUploader
                withIcon={false}
                key={fotoKey}
                singleImage={true}
                buttonText="Seleccione una imagen del curso"
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
                color="primary"
                size="large"
                style={style.submit}
                onClick={guardarCursoEvento}
              >
                Guardar Curso
              </Button>
            </Grid>
          </Grid>
        </form>
      </div>
    </Container>
  );
};

export default NuevoCurso;
