'use strict';
//comentario de prueba
var globalPath = $('base').attr('href');
var AccountSignin = function () {
    var validator;
    var handleForm = function (e) {
        var form = document.querySelector('#kt_sign_in_form');
        var submitButton = document.querySelector('#kt_sign_in_submit');
        validator = FormValidation.formValidation(
            form,
            {
                fields: {
                    'Username': {
                        validators: {
                            notEmpty: {
                                message: 'Usuario es obligatorio'
                            }
                        }
                    },
                    'Password': {
                        validators: {
                            notEmpty: {
                                message: 'Contraseña es obligatorio'
                            }
                        }
                    }
                },
                plugins: {
                    trigger: new FormValidation.plugins.Trigger(),
                    bootstrap: new FormValidation.plugins.Bootstrap5({
                        rowSelector: '.fv-row',
                        eleValidClass: '',
                        eleInvalidClass: '',
                    })
                }
            }
        );
        submitButton.addEventListener('click', function (e) {
            e.preventDefault();
            validator.validate().then(function (status) {
                if (status == 'Valid') {
                    submitButton.setAttribute('data-kt-indicator', 'on');
                    submitButton.disabled = true;
                    setTimeout(function () {
                        $.ajax({
                            type: 'POST',
                            dataType: 'json',
                            url: $('#kt_sign_in_form').attr('action'),
                            xhrFields: {
                                withCredentials: true
                            },
                            data: $('#kt_sign_in_form').serializeObject(),
                            success: function (data) {
                                if (data.succeeded) {
                                    //$(window).attr('location', globalPath);
                                    /*
                                     * Permite que el navegador no pueda dar click en el boton atras e ir nuevamente al login
                                     */
                                    window.location.replace(globalPath)
                                } else {
                                    submitButton.removeAttribute('data-kt-indicator');
                                    submitButton.disabled = false;
                                    messageError(data.message);
                                }
                            },
                            error: function (jqXHR, textStatus, errorThrown) {
                                submitButton.removeAttribute('data-kt-indicator');
                                submitButton.disabled = false;
                                messageError(errorThrown);
                            }
                        });
                        
                    }, 2000);
                } else {
                    messageError('Lo sentimos, parece que se han detectado algunos errores. Vuelve a intentarlo.');
                }
            });
        });
    }
    return {
        init: function () {
            handleForm();
        }
    };
}();
KTUtil.onDOMContentLoaded(function () {
    AccountSignin.init();
});