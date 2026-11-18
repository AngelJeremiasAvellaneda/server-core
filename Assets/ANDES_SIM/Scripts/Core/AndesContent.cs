namespace AndesSim
{
    // Textos exactos del documento. No modificar el contenido narrativo sin revisar el guion.
    public static class AndesContent
    {
        public const string WelcomeTitle = "SISTEMA DE DIAGNÓSTICO";
        public const string WelcomeLine1 = "Bienvenido al centro de operaciones.";
        public const string WelcomeLine2 = "Se iniciará un análisis de la infraestructura tecnológica del BANCO ANDINO.";
        public const string WelcomeButton = "INICIAR DIAGNÓSTICO";

        public const string AlertTitle = "SISTEMA DE MONITOREO";
        public const string AlertLine1 = "Se ha detectado actividad irregular en la infraestructura bancaria.";
        public const string AlertLine2 = "Se requiere una inspección de seguridad.";
        public const string AlertStatus = "ALERTA DETECTADA";

        public const string AccessTitle = "REGISTRO DE ACCESO";
        public const string AccessLine = "Se ha identificado una sesión en el entorno de operaciones SWIFT.";
        public const string AccessTime = "20:36";
        public const string AccessDescription = "El acceso se produjo fuera del horario habitual.";
        public const string AccessClassification = "ACCESO SOSPECHOSO";
        public const string AccessDetailTitle = "DETALLE DEL EVENTO";
        public static readonly string[] AccessDetailLines =
        {
            "Hora: 20:36.",
            "Sistema: entorno de operaciones SWIFT.",
            "Condición: fuera del horario habitual.",
            "Estado: requiere investigación."
        };

        public const string OpsTitle = "MONITOREO DE OPERACIONES";
        public const string OpsLine = "Se han identificado órdenes de transferencia sospechosas.";
        public const string OpsCount = "36 ÓRDENES";
        public const string OpsValue = "US$951 MILLONES";
        public const string OpsNote = "Se requiere verificar la legitimidad de las instrucciones.";
        public static readonly string[] OpsDetailLines =
        {
            "Las órdenes identificadas corresponden a actividad fraudulenta representada en el escenario.",
            "Es necesario investigar los sistemas relacionados con su procesamiento."
        };

        public const string PrinterTitle = "SISTEMA DE IMPRESIÓN";
        public const string PrinterLine1 = "No se están generando las confirmaciones de las operaciones.";
        public const string PrinterLine2 = "Se ha detectado una anomalía en el proceso de impresión.";
        public const string PrinterStatus = "REQUIERE INSPECCIÓN";
        public const string PrinterDetailTitle = "DETALLE DE LA ANOMALÍA";
        public static readonly string[] PrinterDetailLines =
        {
            "Componente: impresora de confirmaciones.",
            "Evento: ausencia de confirmaciones.",
            "Condición: falla no atendida oportunamente.",
            "Importancia: evidencia relevante para la investigación."
        };
        public const string PrinterNote = "Una falla operativa puede ocultar señales importantes de un incidente de seguridad.";

        public const string LogsTitle = "ANÁLISIS DE REGISTROS";
        public const string LogsLine1 = "La reconstrucción del incidente presenta dificultades.";
        public const string LogsLine2 = "La información disponible está incompleta y no permite restablecer la trazabilidad total.";
        public const string LogsStatus = "REGISTROS INCOMPLETOS";

        public const string ReconstructionTitle = "RECONSTRUCCIÓN DEL INCIDENTE";
        public const string ReconstructionLine = "Las anomalías detectadas están relacionadas dentro del escenario.";
        public const string ReconstructionNote = "Las conexiones mostradas son una representación visual del análisis, no un hecho confirmado.";

        public const string MissionTitle = "OBJETIVO DE INVESTIGACIÓN";
        public const string MissionMain = "LOCALIZAR EL ORIGEN DEL INCIDENTE";
        public const string MissionDescription = "Identifica los componentes de la infraestructura relacionados con la actividad irregular.";
        public const string MissionButton = "COMENZAR INVESTIGACIÓN";

        public const string ServerTitle = "SISTEMA DE DIAGNÓSTICO";
        public const string ServerName = "SERVIDOR DE OPERACIONES SWIFT";
        public const string ServerLine1 = "Se han detectado indicadores relacionados con la actividad irregular.";
        public const string ServerLine2 = "Este componente requiere una inspección detallada.";
        public const string ServerPriorityLabel = "Prioridad:";
        public const string ServerPriority = "ALTA";

        public const string HandTitle = "OBJETIVO IDENTIFICADO";
        public const string HandLine1 = "Servidor de operaciones SWIFT.";
        public const string HandLine2 = "Selecciona el componente para iniciar su inspección.";

        public const string SelectedTitle = "COMPONENTE SELECCIONADO";
        public const string SelectedLine1 = "Servidor de operaciones SWIFT.";
        public const string SelectedLine2 = "Preparando inspección detallada...";

        public const string Continue = "CONTINUAR";
        public const string Close = "CERRAR";
        public const string ViewDetail = "VER DETALLE";
    }
}
