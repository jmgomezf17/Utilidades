/**
 * Autor Juan Manuel Gómez
 * Plugin de utilidades para carga de controles, mensajes, ayuda, modales
 * Requiere jQuery, bootstrap
 * Creación: 2016-06-07
 * Modificado: 2016-04-01
**/


(function ($) {

    /**
     * Carga un control <select> con un arreglo json
     * @param: datos -> Arreglo json
    **/
    $.fn.loadData = function (options) {

        var control = $(this);

        defaults = {
            datos: {}
        }

        var settings = $.extend({}, defaults, options);

        control.find('option').remove();
        control.append("<option value='-1'>-- Seleccione --</option>");

        $.each(settings.datos, function (indice, valor) {
            control.append("<option value='" + indice + "'>" + valor + "</option>");
        });
    }

    /**
     * Carga un control <select> con un arreglo json
     * @parametros: Url -> Dirección del controlador/accion
     *              param -> Lista de parametros al controlador
    **/
    $.fn.loadDataAjax = function (options) {

        var control = $(this);
        // Valores por defecto
        var settings = {
            Url: "",
            param: {}
        };

        // Unimos los valores pasados por parametros con los por defecto
        if (options) { $.extend(settings, options); }

        var urlAjax = settings.Url;

        // Limpiamos el select
        control.find('option').remove();
        // Agregamos la opcion seleccion
        control.append("<option value='-1'>-- Seleccione --</option>");

        $.ajax({
            data: settings.param,
            url: urlAjax,
            async: false,
            cache: true,
            type: 'POST',
            dataType: 'json',

            beforeSend: function () {
                control.prop('disabled', true);
            },
            success: function (data) {

                $(data).each(function (i, item) {
                    control.append('<option value="' + item.codigo + '">' + item.nombre + '</option>');
                });

                control.prop('disabled', false);
            },
            error: function (jqXHR) {
                console.log(jqXHR.responseText);
                alert('Ocurrio un error en el servidor ..' + jqXHR.responseText);
                control.prop('disabled', false);
            }
        });

    }


    /**
     * Crea una ventana modal para mostrar mensajes de (alerta, notificacion)
     * @parametros: titulo -> Titulo de la ventana modal
     *              mensaje -> Mensaje de la alerta a mostrar 
    **/
    $.alerta = function (options) {
        
        var settings = {
            titulo: 'Modal title',
            mensaje: 'Aqui va el html',
            aceptar: function () {},
            alerta: false,
            informacion: false,
            confirmacion: false
        };

        // integramos los valores pasados por parametros con los por defecto
        if (options) { $.extend(settings, options); }

        var nombreContenedor = 'contenMensaje';
        var nombreModal = 'mdlMensaje';
        var imageBase64 = 'icono-alerta';

        var mostrarCancelar = true;
        var eventCerrar = '';
        
        if (settings.alerta == false && settings.informacion == false && settings.confirmacion == false) {
            settings.alerta = true;
        }


        if (settings.alerta == true) {

            imageBase64 = 'icono-alerta';
            mostrarCancelar = false;
            eventCerrar = 'data-dismiss="modal"';

        } else if (settings.informacion == true) {

            imageBase64 = 'icono-informacion';
            mostrarCancelar = false;
            eventCerrar = 'data-dismiss="modal"';

        } else if (settings.confirmacion == true) {

            imageBase64 = 'icono-confirmacion';

            $("#btnMdlAceptar").click(function () {
                settings.aceptar;
            });

        }

        /* Si existe la ventana se borra */
        if ($("#" + nombreContenedor).length > 0) {
            $("#" + nombreContenedor).remove();
        }

        /* Generamos el html de la ventana modal*/
        var ventana = '<div id="' + nombreContenedor + '">';
            ventana += '    <div id="' + nombreModal + '" class="modal fade bs-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="modal-title">';
            ventana += '        <div class="modal-dialog" role="document">';
            ventana += '            <div class="modal-content">';
            ventana += '                <div class="modal-header">';
            ventana += '                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>';
            ventana += '                    <h3 class="modal-title" id="modal-title">' + settings.titulo + '</h3>';
            ventana += '                </div>';
            ventana += '                <div id="modal-body" class="modal-body">';
            ventana += '                    <div class="row">';
            ventana += '                        <div class="col-lg-2 col-md-2 col-xs-4 text-center">';
            ventana += '                            <img src="" class="img-responsive ' + imageBase64 + '" width="70px">';
            ventana += '                        </div>';
            ventana += '                        <div class="col-lg-10 col-md-10 col-xs-8">' + settings.mensaje + '</div>';
            ventana += '                    </div>';
            ventana += '                </div>';
            ventana += '                <div class="modal-footer">';
            if (mostrarCancelar) {
                ventana += '                <button id="btnMdlCancelar" type="button" class="btn btn-default" data-dismiss="modal"><i class="fa fa-times"></i> Cancelar</button>';
            }
            ventana += '                    <button id="btnMdlAceptar" type="button" class="btn btn-default" ' + eventCerrar + '><i class="fa fa-check"></i> Aceptar</button>';
            ventana += '                </div>';
            ventana += '            </div>';
            ventana += '        </div>';
            ventana += '    </div>';
            ventana += '</div>';


        $('body').append(ventana);
        $("#" + nombreModal).modal('show');

        $("#btnMdlAceptar").bind('click', settings.aceptar);
        
        $("#btnMdlAceptar").click(function () {
            $("#btnMdlCancelar").click();
        });
    }

    /**
     * Pinta una lista de tabs
     * @parametros: nombre -> nombre del contenedor de los tabs, es opcional si solo se pinta una vez en el formulario
     *              listaTabs -> Arreglo json con la lista de las opciones, el formato debe ser: { ruta: 'Principal/TabUno', label: 'Pestaña uno' }
     *              activar -> Indica el indice del tab que se debe activar, es opcional, por defecto carga el indice 0 
    **/
    $.fn.crearTabs = function (options) {
        
        var settings = {
            nombre: 'default',
            listaTabs: [],
            activar: 0
        };
        
        // integramos los valores pasados por parametros con los por defecto
        if (options) { $.extend(settings, options); }

        var data_target = settings.nombre + '_contenido';

        if (settings.listaTabs.length > 0) {

            /* Generamos el html de la lista tabs*/
            var html = '<div id="' + settings.nombre + '" class="">';
            html += '   <ul class="nav nav-tabs tabs-up">';

            $(settings.listaTabs).each(function (i, item) { // indice, item
                
                var activar = (i == settings.activar) ? 'class="active"' : '';
                html += '   <li ' + activar + '><a href="' + item.ruta + '" data-target="#' + data_target + '" data-toggle="tabajax" >' + item.label + '</a></li>';
            });

            html += '</ul>';
            html += ' <div class="tab-content">';
            html += '    <div id="' + data_target + '"></div>';
            html += '</div>';
            html += '</div>';

            $(this).html(html);
        } else {
            $(this).html('La lista de tabs encuentra vacía...');
        }

        // Adicionar el evento click a la lista de tabs
        $('[data-toggle="tabajax"]').click(function (e) {
            var $this = $(this),
                loadurl = $this.attr('href'),
                targ = $this.attr('data-target');

            $.get(loadurl, function (data) {
                $(targ).html(data);
            });

            $this.tab('show');
            return false;
        });
       
        //Recorrer los tabs creados y dar click en el activo
        $("#" + settings.nombre + " a").each(function(i){
            if (i == settings.activar) {
                $(this).click();
            }
        });

    }

    /**
     * Crear un mensaje popover sobre un elemento
     * @parametros: elemento -> Nombre del elemento
     *              {mensaje} -> Mensaje a mostrar sobre el elemento
    **/
    $.crearPopover = function (elemento, options) {

        var settings = {
            listaData: [{ key: 'container', valor: 'body' },
                        { key: 'toggle', valor: 'popover' },
                        { key: 'placement', valor: 'top' }],
            mensaje: 'Mensaje de validación'
        };

        // integramos los valores pasados por parametros con los por defecto
        if (options) { $.extend(settings, options); }

        // agregamos los data de popover al elemento
        $(settings.listaData).each(function (index, item) {
            $(elemento).data(item.key, item.valor);
        });

        // agregamos el mensaje al popover
        $(elemento).data('content', settings.mensaje);

        // agregamos clase de error al elemento
        $(elemento).addClass('error-valida');

        // mostramos el popover sobre el elemento
        $(elemento).popover('show');
    }


    /**
     * Validar los controles requeridos y los tipos de datos
     * @parametros: contenedor -> Nombre del contenedor de los elementos a validar, sino se envia por parametro, toma por defecto el body
    **/
    $.validarControles = function (options) {

        if (options.contenedor != '' && options.contenedor != undefined) {
            options.contenedor = "#" + options.contenedor;
        }

        var settings = {
            contenedor: 'body'
        };

        // integramos los valores pasados por parametros con los por defecto
        if (options) { $.extend(settings, options); }

        var validacionOk = true;

        // Validamos los elementos requeridos
        $(settings.contenedor + ' [data-requerido]').each(function (index, elemento) {
            
            var tipoElemento = $(elemento).prop('type');
            var data_tipo_dato = $(elemento).data('tipo-dato');
            

            //console.log(this);
            //alert(this.id + ', tipo: ' + tipoElemento);
            //alert($(elemento).val());

            switch (tipoElemento) {
                case 'text':

                    $(elemento).val() == '' ? ($(elemento).addClass('error-valida'), validacionOk = false) : '';

                    $(elemento).keyup(function () {
                        this.value != '' ? $(elemento).removeClass('error-valida') : '';
                    });

                    $(elemento).change(function () {
                        this.value != '' ? $(elemento).removeClass('error-valida') : '';
                    });

                    break;

                case 'select-one':

                    $(elemento).val() <= 0 || $(elemento).val() <= "" ? ($(elemento).addClass('error-valida'), validacionOk = false) : '';

                    $(elemento).change(function () {
                        this.value > 0 ? $(elemento).removeClass('error-valida') : '';
                    });

                    break;

                case 'checkbox':

                    $(elemento).is(':checked') == false ? ($(elemento).addClass('error-valida'), validacionOk = false) : '';

                    $(elemento).change(function () {
                        this.checked == true ? $(elemento).removeClass('error-valida') : '';
                    });

                    break;

                case 'radio':

                    $(elemento).is(':checked') == false ? ($(elemento).addClass('error-valida'), validacionOk = false) : '';

                    $(elemento).change(function () {
                        this.checked == true ? $(elemento).removeClass('error-valida') : '';
                    });

                    break;

                default:

                    $(elemento).val() == '' ? ($(elemento).addClass('error-valida'), validacionOk = false) : '';

                    $(elemento).keyup(function () {
                        this.value != '' ? $(elemento).removeClass('error-valida') : '';
                    });

                    $(elemento).change(function () {
                        this.value != '' ? $(elemento).removeClass('error-valida') : '';
                    });

                    break;
            }
            

        });



        //$('[data-toggle="tooltip"]').tooltip();

        // validamos los elementos con tipo de dato
        $(settings.contenedor + ' [data-tipo-dato]').each(function (index, elemento) {

            var data_tipo_dato = $(elemento).data('tipo-dato');
            var valor = $(elemento).val();

            if (data_tipo_dato != '' && data_tipo_dato != undefined && valor != '') {

                switch (data_tipo_dato) {
                    case 'numerico':

                        if (/[^0-9]/g.test(valor) == true) {

                            $.crearPopover(elemento, { mensaje: 'El dato debe ser numérico.' })
                            validacionOk = false;

                        } else {

                            $(elemento).removeClass('error-valida');
                            $(elemento).popover('destroy');
                        }

                        break;

                    case 'texto':

                        if (/[^A-Z a-zÑñáéíóúÁÉÍÓÚ]/g.test(valor) == true) {

                            $.crearPopover(elemento, { mensaje: 'El dato debe ser solo texto.' })
                            validacionOk = false;

                        } else {

                            $(elemento).removeClass('error-valida');
                            $(elemento).popover('destroy');
                        }

                        break;

                    case 'alfanumerico':

                        if (/[^0-9A-Z a-zÑñáéíóúÁÉÍÓÚ]/g.test(valor) == true) {

                            $.crearPopover(elemento, { mensaje: 'El dato debe ser alfanumérico.' })
                            validacionOk = false;

                        } else {

                            $(elemento).removeClass('error-valida');
                            $(elemento).popover('destroy');
                        }

                        break;

                    default:
                        break;
                }

            }
            
        });

        /* Validar el tipo de dato si existe */

        return validacionOk;
    }

    /**
     * Cargar una pagina externa dentro de una ventana modal
     * @param:  nombreModal -> Nombre de la ventana modal
     *          urlExt -> Url de la pagina externa
     *          titulo -> Titulo de la ventana modal
     *          altura -> Altura de la ventana modal
    **/
    $.fn.modalPaginaExt = function (options) {
        
        var control = $(this);

        defaults = {
            nombreModal:'default_ext',
            urlExt: '',
            titulo: '',
            altura: '350px'
        }

        var settings = $.extend({}, defaults, options);

        var nombreContenedor = 'contenModalExt';

        /* Generamos el html de la ventana modal*/
        var ventana = '<div id="' + nombreContenedor + '">';
        ventana += '    <div id="' + settings.nombreModal + '" class="modal fade bs-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="modal-title"  >';
        ventana += '        <div class="modal-dialog" role="document">';
        ventana += '            <div class="modal-content">';
        ventana += '                <div class="modal-header">';
        ventana += '                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>';
        ventana += '                    <h3 class="modal-title" id="modal-title">' + settings.titulo + '</h3>';
        ventana += '                </div>';
        ventana += '                <div id="modal-body-externa" class="modal-body">';
        ventana += '                <iframe width="100%" height="' + settings.altura + '" id="idframeAyuda" name="idframeAyuda" frameborder="0" ></iframe>';
        ventana += '                </div>';
        ventana += '            </div>';
        ventana += '        </div>';
        ventana += '    </div>';
        ventana += '</div>';
        
        $('body').append(ventana);

        $("#idframeAyuda").attr("src", settings.urlExt);
        $("#" + settings.nombreModal).modal('show');

    }

    /**
     * Cargar una vista html
     * @param:  nombreModal -> Nombre de la ventana modal
     *          urlExt -> Url de la pagina externa
     *          titulo -> Titulo de la ventana modal
     *          altura -> Altura de la ventana modal
     *          param -> Lista de parametros JSON
    **/
    $.fn.modalVista = function (options) {

        var control = $(this);

        defaults = {
            nombreModal: 'default_vista',
            urlExt: '',
            titulo: 'Titulo de la vista',
            altura: '350px',
            param: {}
        }

        var settings = $.extend({}, defaults, options);

        var nombreContenedor = 'contenModalVista';

        /* Generamos el html de la ventana modal*/
        var ventana = '<div id="' + nombreContenedor + '">';
        ventana += '    <div id="' + settings.nombreModal + '" class="modal fade bs-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="modal-title"  >';
        ventana += '        <div class="modal-dialog" role="document">';
        ventana += '            <div class="modal-content">';
        ventana += '                <div class="modal-header">';
        ventana += '                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>';
        ventana += '                    <h3 class="modal-title" id="modal-title-vista">' + settings.titulo + '</h3>';
        ventana += '                </div>';
        ventana += '                <div id="modal-body-vista" class="modal-body">';
        //ventana += '                <iframe width="100%" height="' + settings.altura + '" id="idframeModal" name="idframeModal" frameborder="0" ></iframe>';
        ventana += '                </div>';
        ventana += '            </div>';
        ventana += '        </div>';
        ventana += '    </div>';
        ventana += '</div>';

        $('body').append(ventana);

        $("#modal-body-vista").load(settings.urlExt, settings.param);
        //$("#idframeModal").attr("src", settings.urlExt);
        $("#" + settings.nombreModal).modal('show');
    }

    /**
     * Cargar una vista html
     * @param:  nombreModal -> Nombre de la ventana modal
     *          urlExt -> Url de la pagina externa
     *          titulo -> Titulo de la ventana modal
     *          altura -> Altura de la ventana modal
     *          param -> Lista de parametros JSON
     *          paginar -> Indica si desea paginar la grilla
    **/
    $.fn.cargarGrilla = function (options) {

        var control = $(this);

        defaults = {
            nombre: 'TblResultado',
            url: '',
            param: {},
            buscar: false,
            scrollX: false,
            paginar: true,
            numRegistros: 10,
            noDatos: {
                mostar: false,
                mensaje: 'No hay registros disponibles'
            },
            columnas:[]
        }

        var settings = $.extend({}, defaults, options);

        $(control).html('<table class="table nowrap" width="95%" id="' + settings.nombre + '"></table><br/>');

        // Enviamos la peticion para consultar la lista de registros a pintar

            $.ajax({
                    type: "POST",
                    url: settings.url,
                    data: JSON.stringify(settings.param),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    processData: true,
                    success: function (data, status, jqXHR) {
                        
                        var respuesta = JSON.parse(data.d);

                        if (settings.noDatos.mostar == true && respuesta.length == 0) {
                            Alerta(settings.noDatos.mensaje);
                        }

                        $('#' + settings.nombre).dataTable({
                            data: respuesta,                    // Lista de datos
                            scrollX: settings.scrollX,          // Muestra el scroll en el eje X
                            destroy: true,                      // Destruye la instancia si existe
                            bFilter: settings.buscar,           // Campo de busqueda sobre el resultado
                            columns: settings.columnas,         // arreglo con la lista de columnas
                            paging: settings.paginar,           // Indica si se desea paginar la grilla
                            bLengthChange: false,               // Ocultar el combo del numero de registros por pagina
                            pageLength: settings.numRegistros,  // Número de registros por pagina

                            language: {
                                "lengthMenu": "Mostrando _MENU_ Registros por página",
                                "zeroRecords": "¡Atención! - No hay registros disponibles",
                                "info": "Mostrando página _PAGE_ de _PAGES_",
                                "infoEmpty": "No hay registros disponibles",
                                "infoFiltered": "(filtered from _MAX_ total records)",
                                "search": "Buscar:",
                                "oPaginate": {
                                    "sFirst": "Primero",
                                    "sLast": "Último",
                                    "sNext": "Siguiente",
                                    "sPrevious": "Anterior"
                                }
                            }
                        });

                    },
                    error: function (xhr) {
                        alert(xhr.responseText);
                    }
            //});


               
        });

    }

    /**
     * Realiza una peticion ajax
     * @param:  url -> Url de la pagina externa
     *          titulo -> Titulo de la ventana modal
     *          altura -> Altura de la ventana modal
     *          param -> Lista de parametros JSON
     *          paginar -> Indica si desea paginar la grilla
    **/
    $.peticionAjax = function (options) {

        var defaults = {
            url: '',
            param: {},
            metodo: 'GET',
            respuesta: function () { }
        };

        var settings = $.extend({}, defaults, options);

        $.ajax({
            url: settings.url,
            data: settings.param,
            async: false,
            cache: true,
            type: settings.metodo,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            crossDomain: true,
            success: settings.respuesta,
            error: function (jqXHR) {
                console.log(jqXHR.responseText);
                alert(jqXHR.responseText);
            }
        });

    }

}(jQuery));

//===============================================================================================================================
$(document).ready(function () {

    $('[data-tipo-dato="texto"]').on("keyup blur", function () {
        this.value = (this.value + '').replace(/[^A-Z a-zÑñáéíóúÁÉÍÓÚ]/g, '');
    });

    $('[data-tipo-dato="alfanumerico"]').on("keyup blur", function () {
        this.value = (this.value + '').replace(/[^0-9A-Z a-zÑñáéíóúÁÉÍÓÚ]/g, '');
    });

    $('[data-tipo-dato="numerico"]').on("keyup blur", function () {
        this.value = (this.value + '').replace(/[^0-9]/g, '');
    });

    //$('.texto-numero-guion').on("keyup blur", function () {
    //    this.value = (this.value + '').replace(/[^0-9A-Z a-zÑñáéíóúÁÉÍÓÚ-]/g, '');
    //});

    //$('.texto-numero-punto').on("keyup blur", function () {
    //    this.value = (this.value + '').replace(/[^0-9A-Z a-zÑñáéíóúÁÉÍÓÚ\.]/g, '');
    //});

    //$('.texto-numero-sintildes').on("keyup blur", function () {
    //    this.value = (this.value + '').replace(/[^0-9A-Z a-z]/g, '');
    //});

});

//===============================================================================================================================

/*
* Mensajes de alerta
*/

var Alerta = function (_mensaje) {
    $.alerta({
        titulo: "¡ATENCIÓN!",
        mensaje: _mensaje,
        alerta: true
    });
}

var Informacion = function (_mensaje) {
    $.alerta({
        titulo: "¡INFORMACIÓN!",
        mensaje: _mensaje,
        informacion: true
    });
}

var Confirmacion = function (_mensaje, _eventoAceptar) {
    $.alerta({
        titulo: "¡CONFIRMACIÓN!",
        mensaje: _mensaje,
        confirmacion: true,
        aceptar: _eventoAceptar
    });
}

var ValidarRequeridos = function (_contenedor) {
    return $.validarControles({ contenedor: _contenedor });
}