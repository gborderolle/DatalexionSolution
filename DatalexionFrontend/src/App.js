import React, { Suspense } from "react";
import { HashRouter, Route, Routes } from "react-router-dom";

// redux imports
import { useSelector } from "react-redux";

import "./scss/style.scss";

const loading = (
  <div className="pt-3 text-center">
    <div className="sk-spinner sk-spinner-pulse"></div>
  </div>
);

const LoginAdmin = React.lazy(() => import("./views/pages/login/LoginAdmin"));
const LoginDelegados = React.lazy(() =>
  import("./views/pages/login/LoginDelegados")
);
const DefaultLayout = React.lazy(() => import("./defaultLayout/DefaultLayout"));

function App() {
  //#region Consts ***********************************

  const isLoggedIn = useSelector((state) => state.auth.isLoggedIn);

  //#endregion Consts ***********************************

  //#region Hooks ***********************************

  //#endregion Hooks ***********************************

  return (
    <HashRouter>
      <Suspense fallback={loading}>
        <Routes>
          {!isLoggedIn && (
            <Route path="/login-delegados" element={<LoginDelegados />} />
          )}
          {!isLoggedIn && <Route path="*" element={<LoginAdmin />} />}
          {isLoggedIn && <Route path="*" element={<DefaultLayout />} />}
        </Routes>
      </Suspense>
    </HashRouter>
  );
}

export default App;
