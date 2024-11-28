import logo from "../../../assets/common/logo-title.svg";
import image from "../../../assets/auth/login-img.svg";
import LoginForm from "./Form";

function App() {
  return (

        <div className="px-12 flex-1 flex flex-col justify-center">
          <h1 className="font-bold text-4xl">Sign in</h1>
          <p className="text-base text-gray-400 pt-1">
            Please login to continue to your account.
          </p>
          <LoginForm />
        </div>
  );
}

export default App;