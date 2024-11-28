import SignUpForm from "./Form";

function App() {
  return (
        <div className="px-12 flex-1 flex flex-col justify-center">
          <h1 className="font-bold text-4xl">Sign up</h1>
          <p className="text-base text-gray-400 pt-1">
            Please complete the form to continue.
          </p>
          <SignUpForm />
        </div>
  );
}

export default App;