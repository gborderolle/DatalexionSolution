import React from "react";

const LoginDelegados = React.lazy(() =>
  import("./views/pages/login/LoginDelegados")
);
const FormStart = React.lazy(() =>
  import("./components/delegadosSide/steps/FormStart")
);
const FormParty1 = React.lazy(() =>
  import("./components/delegadosSide/steps/FormParty1")
);
const FormSlate1 = React.lazy(() =>
  import("./components/delegadosSide/steps/FormSlate1")
);
const FormExtras1 = React.lazy(() =>
  import("./components/delegadosSide/steps/FormExtras1")
);
const FormSummary = React.lazy(() =>
  import("./components/delegadosSide/steps/FormSummary")
);
const MenuData = React.lazy(() =>
  import("./components/adminSide/menu/MenuData")
);
const MenuAdmin = React.lazy(() =>
  import("./components/adminSide/data/MenuAdmin")
);
const AdminMenu = React.lazy(() =>
  import("./components/adminSide/menu/AdminMenu")
);
const Dashboard = React.lazy(() =>
  import("./components/adminSide/dashboard/Dashboard")
);
const MapsDashboard = React.lazy(() =>
  import("./components/adminSide/maps/MapsDashboard")
);
const DelegatesMenu = React.lazy(() =>
  import("./components/adminSide/delegados/DelegadosMenu")
);
const LogsTable = React.lazy(() =>
  import("./components/adminSide/logs/LogsMenu")
);

const routes = [
  { path: "/", exact: true, name: "Home" },
  {
    path: "/login-delegados",
    name: "Login delegados",
    element: LoginDelegados,
  },
  { path: "/formStart", name: "Formulario", element: FormStart },
  { path: "/formSlate1", name: "Formulario", element: FormSlate1 },
  { path: "/formParty1", name: "Formulario", element: FormParty1 },
  { path: "/formExtras1", name: "Formulario", element: FormExtras1 },
  { path: "/formSummary", name: "Formulario", element: FormSummary },
  { path: "/menu-data", name: "Menú de datos", element: MenuData },
  { path: "/menu-admin", name: "Menú admin", element: MenuAdmin },
  { path: "/admin", name: "Menú de administración", element: AdminMenu },
  { path: "/dashboard", name: "Dashboard", element: Dashboard },
  { path: "/maps", name: "Distribución", element: MapsDashboard },
  { path: "/delegates", name: "Delegados", element: DelegatesMenu },
  { path: "/logs", name: "Logs", element: LogsTable },
];

export default routes;
