import { useNavigate } from "react-router-dom";
import List from "../components/List";
import { token } from "../signals/AuthSignal";
import { count, decrement, increment } from "../signals/UserSignal";
import { useEffect } from "react";

function Home() {
  const navigate = useNavigate();

  useEffect(() => {
    if (token.value == null) {
      navigate("/login", { replace: true });
    }
  });

  return (
    <div className="flex flex-col justify-center items-center h-svh">
      <h3 className="text-3xl font-medium ">{count}</h3>
      <div className="flex flex-row mt-4 space-x-2">
        <button
          className="py-2 px-4 bg-blue-500 text-white border-2 border-blue-500 rounded hover:bg-transparent hover:text-blue-500"
          onClick={increment}
        >
          Increment
        </button>
        <button
          className="py-2 px-4 bg-red-500 text-white border-2 border-red-500 rounded hover:bg-transparent hover:text-red-500"
          onClick={decrement}
        >
          Decrement
        </button>
      </div>
      <div className="flex flex-col mt-4">
        <List />
      </div>
    </div>
  );
}

export default Home;
