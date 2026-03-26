import { useEffect, useState } from "react"
import Login from "./components/Login"
import Register from "./components/Register"
import Home from "./components/Home"

const App = () => {
  const [page, setPage] = useState(() => {
    const savedToken = localStorage.getItem("token");
    const savedPage = localStorage.getItem("page");

    if (savedToken) return "home"
    return savedPage || "login"
  });

  useEffect(() => {
    localStorage.setItem("page", page)

    const token = localStorage.getItem("token");
    if (page === "home" && !token) {
      setPage("login");
    }

  }, [page])

  const renderPage = () => {
    switch (page) {
      case "login":
        return <Login setPage={setPage} />
      case "register":
        return <Register setPage={setPage} />
      case "home":
        return <Home setPage={setPage} />
      default:
        return <Login setPage={setPage} />
    }
  }

  return <>{renderPage()}</>

}

export default App