import logo from '../../../assets/common/logo-title.svg';
import image from '../../../assets/auth/login-img.svg';
import LoginForm from './form';

function App() {
  return (
    <main className="grid md:grid-cols-2 min-h-screen">
      <section className="flex flex-col">
        <img src={logo} alt="notepad ai" className="p-4  w-44" />
        <div className=" px-12 pt-[30%]">
          <h1 className=" font-bold text-4xl">Sign in</h1>
          <p className="text-base text-gray-400 pt-1">
            Please login to continue to your account.
          </p>
          <LoginForm />
        </div>
      </section>
      <section className="hidden md:grid bg-black items-center">
        <div className="p-4">
          <img src={image} alt="" className=" opacity-80 border-opacity-10" />
        </div>
      </section>
    </main>
  );
}

export default App;
