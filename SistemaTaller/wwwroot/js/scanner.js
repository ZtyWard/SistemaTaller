/* =========================================================
   AXIS - SCANNER DE PRODUCTOS
   ========================================================= */

document.addEventListener("DOMContentLoaded", function () {

    const readerElement =
        document.getElementById("qr-reader");

    const scannerStatus =
        document.getElementById("scannerStatus");

    const scannerResult =
        document.getElementById("scannerResult");

    const scannerError =
        document.getElementById("scannerError");

    const scannerErrorMessage =
        document.getElementById("scannerErrorMessage");

    const codigoManual =
        document.getElementById("codigoManual");

    const btnBuscarManual =
        document.getElementById("btnBuscarManual");

    const btnEscanearOtro =
        document.getElementById("btnEscanearOtro");

    const btnVerProducto =
        document.getElementById("btnVerProducto");

    const buscarUrl =
        window.scannerConfig.buscarUrl;


    let qrScanner = null;

    let procesando = false;


    // =====================================================
    // UTILIDADES
    // =====================================================

    function mostrarError(mensaje) {

        scannerErrorMessage.textContent =
            mensaje ||
            "No se encontró ningún producto.";

        scannerError.hidden = false;
    }


    function ocultarError() {

        scannerError.hidden = true;
    }


    function ocultarResultado() {

        scannerResult.hidden = true;
    }


    function actualizarEstado(texto) {

        scannerStatus.innerHTML = `
            <span class="scanner-status-dot"></span>
            ${texto}
        `;
    }


    function formatearPrecio(valor) {

        if (
            valor === null ||
            valor === undefined
        ) {
            return "—";
        }

        return new Intl.NumberFormat(
            "es-CR",
            {
                style: "currency",
                currency: "CRC",
                minimumFractionDigits: 2
            }
        ).format(valor);
    }


    // =====================================================
    // MOSTRAR PRODUCTO
    // =====================================================

    function mostrarProducto(producto) {

        ocultarError();

        document.getElementById(
            "resultadoNombre"
        ).textContent =
            producto.nombre || "Producto";


        document.getElementById(
            "resultadoCodigo"
        ).textContent =
            producto.codigo || "Sin código";


        const estado =
            document.getElementById(
                "resultadoEstado"
            );

        estado.textContent =
            producto.activo
                ? "● Activo"
                : "● Inactivo";


        document.getElementById(
            "infoCodigo"
        ).textContent =
            producto.codigo || "—";


        document.getElementById(
            "infoCodigoBarras"
        ).textContent =
            producto.codigoBarras || "—";


        document.getElementById(
            "infoCategoria"
        ).textContent =
            producto.categoria || "Sin categoría";


        document.getElementById(
            "infoStock"
        ).textContent =
            producto.stock ?? "0";


        document.getElementById(
            "infoStockMinimo"
        ).textContent =
            producto.stockMinimo ?? "0";


        document.getElementById(
            "infoPrecioVenta"
        ).textContent =
            formatearPrecio(
                producto.precioVenta
            );


        document.getElementById(
            "infoDescripcion"
        ).textContent =
            producto.descripcion ||
            "Sin descripción.";


        // =================================================
        // IMAGEN
        // =================================================

        const imagen =
            document.getElementById(
                "resultadoImagen"
            );

        const placeholder =
            document.getElementById(
                "imagenPlaceholder"
            );

        const inicial =
            document.getElementById(
                "imagenInicial"
            );


        imagen.hidden = true;

        placeholder.hidden = false;


        inicial.textContent =
            (
                producto.nombre ||
                "P"
            )
                .trim()
                .charAt(0)
                .toUpperCase();


        if (
            producto.imagenUrl &&
            producto.imagenUrl.trim() !== ""
        ) {

            imagen.onload = function () {

                placeholder.hidden = true;

                imagen.hidden = false;

            };


            imagen.onerror = function () {

                placeholder.hidden = false;

                imagen.hidden = true;

            };


            imagen.src =
                producto.imagenUrl;
        }


        // =================================================
        // VER PRODUCTO
        // =================================================

        btnVerProducto.href =
            "/Producto/Details/" +
            producto.idProducto;


        scannerResult.hidden = false;


        actualizarEstado(
            "Producto encontrado"
        );
    }


    // =====================================================
    // BUSCAR PRODUCTO
    // =====================================================

    async function buscarProducto(codigo) {

        if (!codigo) {
            return;
        }


        codigo =
            codigo.trim();


        if (!codigo) {
            return;
        }


        if (procesando) {
            return;
        }


        procesando = true;


        actualizarEstado(
            "Consultando producto..."
        );


        ocultarError();


        try {

            const url =
                buscarUrl +
                "?codigo=" +
                encodeURIComponent(codigo);


            const response =
                await fetch(url, {
                    method: "GET",
                    headers: {
                        "Accept":
                            "application/json"
                    }
                });


            if (!response.ok) {

                let mensaje =
                    "No se encontró ningún producto con ese código.";


                try {

                    const errorData =
                        await response.json();

                    if (
                        errorData &&
                        errorData.mensaje
                    ) {
                        mensaje =
                            errorData.mensaje;
                    }

                }
                catch {
                    // No hacer nada.
                }


                ocultarResultado();

                mostrarError(mensaje);

                actualizarEstado(
                    "Producto no encontrado"
                );

                return;
            }


            const producto =
                await response.json();


            mostrarProducto(producto);

        }
        catch (error) {

            console.error(
                "Error al consultar producto:",
                error
            );


            mostrarError(
                "No fue posible comunicarse con AXIS."
            );


            actualizarEstado(
                "Error de conexión"
            );

        }
        finally {

            procesando = false;
        }
    }


    // =====================================================
    // QR DETECTADO
    // =====================================================

    async function onScanSuccess(
        decodedText,
        decodedResult
    ) {

        if (procesando) {
            return;
        }


        console.log(
            "QR detectado:",
            decodedText
        );


        // Evitar múltiples lecturas consecutivas.
        procesando = true;


        try {

            if (qrScanner) {

                await qrScanner.stop();

                qrScanner.clear();

            }

        }
        catch (error) {

            console.warn(
                "No fue posible detener el scanner:",
                error
            );

        }


        procesando = false;


        await buscarProducto(
            decodedText
        );
    }


    // =====================================================
    // QR ERROR
    // =====================================================

    function onScanFailure(errorMessage) {

        // No mostrar errores continuamente.
        // html5-qrcode llama esta función mientras
        // todavía no encuentra un QR.
    }


    // =====================================================
    // INICIAR SCANNER
    // =====================================================

    async function iniciarScanner() {

        try {

            ocultarError();

            ocultarResultado();


            actualizarEstado(
                "Solicitando cámara..."
            );


            qrScanner =
                new Html5Qrcode(
                    "qr-reader"
                );


            const config = {

                fps: 10,

                qrbox: function (
                    viewfinderWidth,
                    viewfinderHeight
                ) {

                    const size =
                        Math.min(
                            viewfinderWidth,
                            viewfinderHeight
                        ) * 0.65;

                    return {
                        width: size,
                        height: size
                    };
                },

                aspectRatio: 1.0

            };


            await qrScanner.start(

                {
                    facingMode:
                        "environment"
                },

                config,

                onScanSuccess,

                onScanFailure

            );


            actualizarEstado(
                "Cámara activa · Escanea un QR"
            );

        }
        catch (error) {

            console.error(
                "No fue posible iniciar la cámara:",
                error
            );


            actualizarEstado(
                "Cámara no disponible"
            );


            mostrarError(
                "No se pudo acceder a la cámara. Verifica los permisos del navegador."
            );

        }
    }


    // =====================================================
    // ESCANEAR OTRO
    // =====================================================

    btnEscanearOtro.addEventListener(
        "click",
        async function () {

            ocultarResultado();

            ocultarError();

            codigoManual.value = "";

            await iniciarScanner();

        }
    );


    // =====================================================
    // BÚSQUEDA MANUAL
    // =====================================================

    btnBuscarManual.addEventListener(
        "click",
        function () {

            buscarProducto(
                codigoManual.value
            );

        }
    );


    codigoManual.addEventListener(
        "keydown",
        function (event) {

            if (
                event.key === "Enter"
            ) {

                event.preventDefault();

                buscarProducto(
                    codigoManual.value
                );
            }

        }
    );


    // =====================================================
    // INICIO
    // =====================================================

    iniciarScanner();

});