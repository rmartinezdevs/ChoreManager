import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import HomePage from "../pages/HomePage.tsx";
import ChoresPage from "../pages/ChoresPage.tsx";

export default function AppRoutes() {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<HomePage />} />
        <Route path="/chores" element={<ChoresPage />} />
      </Routes>
    </Router>
  );
}