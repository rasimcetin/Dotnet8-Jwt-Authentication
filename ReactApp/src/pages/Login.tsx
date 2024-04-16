import { useState } from "react";
import { LoginDto, token } from "../signals/AuthSignal";
import axios from "axios";
import { useNavigate } from "react-router-dom";

function Login() {
  const [userName, setUserName] = useState("");
  const [password, setPassword] = useState("");
  const navigate = useNavigate();

  async function login() {
    const loginDto: LoginDto = {
      Username: userName,
      Password: password,
    };

    const response = await axios.post(
      "http://localhost:5137/api/authentication",
      loginDto
    );

    if (response.data == null) return;
    token.value = response.data;
    navigate("/", { replace: true });
  }

  return (
    <div className="flex flex-col justify-center items-center h-svh">
      <div className="flex flex-col justify-center items-center bg-gray-100 rounded-lg p-8">
        <h3 className="text-3xl font-medium text-gray-700">Login</h3>
        <div className="flex flex-col mt-4">
          <p className="text-xl font-medium text-gray-700">Username</p>
          <input
            className="border-2 border-gray-700 rounded-md h-12 w-72 mt-2 py-2 px-4"
            type="text"
            value={userName}
            onChange={(e) => setUserName(e.target.value)}
            placeholder="Username"
          />
        </div>
        <div className="flex flex-col mt-4">
          <p className="text-xl font-medium text-gray-700">Password</p>
          <input
            className="border-2 border-gray-700 rounded-md h-12 w-72 mt-2 py-2 px-4"
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            placeholder="Password"
          />
        </div>
        <button
          className="py-2 px-4 bg-blue-500 text-white border-2 border-blue-500 rounded hover:bg-transparent hover:text-blue-500 mt-4 w-72"
          onClick={login}
        >
          Login
        </button>
      </div>
    </div>
  );
}

export default Login;
