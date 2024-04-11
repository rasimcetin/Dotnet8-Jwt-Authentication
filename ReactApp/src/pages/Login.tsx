function Login() {
  return (
    <div className="flex flex-col justify-center items-center h-svh">
      <h3 className="text-3xl font-medium text-gray-700">Login Page</h3>
      <div className="flex flex-col mt-4">
        <p className="text-xl font-medium text-gray-700">Username</p>
        <input
          className="border-2 border-gray-700 rounded-md h-12 w-72 mt-2 py-2 px-4"
          type="text"
        />
      </div>
      <div className="flex flex-col mt-4">
        <p className="text-xl font-medium text-gray-700">Password</p>
        <input
          className="border-2 border-gray-700 rounded-md h-12 w-72 mt-2 py-2 px-4"
          type="password"
        />
      </div>
      <button className="py-2 px-4 bg-blue-500 text-white border-2 border-blue-500 rounded hover:bg-transparent hover:text-blue-500 mt-4 w-72">
        Login
      </button>
    </div>
  );
}

export default Login;
