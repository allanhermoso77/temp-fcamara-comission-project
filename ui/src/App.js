import { useState } from "react";
import logo from './logo.svg';
import './App.css';
import { calculateCommission } from "./api/comission";

function App() {

  const [form, setForm] = useState({
    localSalesCount: "",
    foreignSalesCount: "",
    averageSaleAmount: "",
  });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const [result, setResult] = useState(null);

  const onChange = (e) => {
    const { name, value } = e.target;
    setForm((f) => ({ ...f, [name]: value }));
  };

  const onSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    setError(null);
    setResult(null);
    try {
      const data = await calculateCommission({
        localSalesCount: Number(form.localSalesCount || 0),
        foreignSalesCount: Number(form.foreignSalesCount || 0),
        averageSaleAmount: Number(form.averageSaleAmount || 0),
      });
      setResult(data);
    } catch (err) {
      setError(err.message || "Request failed");
    } finally {
      setLoading(false);
    }
  };

  return (
    <main className="app">
      <header className="header">
        <img src={logo} className="logo" alt="FCamara Logo" />
        <h1>Commission Calculator</h1>
      </header>
  <section className="card">
    <form onSubmit={onSubmit} noValidate className="form">
      <fieldset className="grid">
        <legend className="legend">Inputs</legend>

        <div className="field">
          <label htmlFor="localSalesCount">Local sales count</label>
          <input
            id="localSalesCount"
            name="localSalesCount"
            type="number"
            inputMode="numeric"
            min="0"
            placeholder="e.g., 10"
            value={form.localSalesCount}
            onChange={onChange}
            required
          />
          <small className="hint">How many local sales you made.</small>
        </div>

        <div className="field">
          <label htmlFor="foreignSalesCount">Foreign sales count</label>
          <input
            id="foreignSalesCount"
            name="foreignSalesCount"
            type="number"
            inputMode="numeric"
            min="0"
            placeholder="e.g., 10"
            value={form.foreignSalesCount}
            onChange={onChange}
            required
          />
          <small className="hint">How many foreign sales you made.</small>
        </div>

        <div className="field">
          <label htmlFor="averageSaleAmount">Average sale amount</label>
          <div className="input-with-prefix">
            <span className="prefix">£</span>
            <input
              id="averageSaleAmount"
              name="averageSaleAmount"
              type="number"
              step="0.01"
              min="0"
              placeholder="e.g., 100.00"
              value={form.averageSaleAmount}
              onChange={onChange}
              required
              aria-describedby="avg-help"
            />
          </div>
          <small id="avg-help" className="hint">Average value per sale.</small>
        </div>
      </fieldset>

      <div className="actions">
        <button type="submit" className="btn" disabled={loading}>
          {loading ? "Calculating…" : "Calculate"}
        </button>
      </div>
    </form>
  </section>

  <section className="card">
    <h2 className="subtitle">Results</h2>

    {error && (
      <div className="alert" role="alert">
        {error}
      </div>
    )}

    <div aria-live="polite" role="status">
      {result ? (
        <table className="results">
          <tbody>
            <tr>
              <th scope="row">Total FCamara commission</th>
              <td>
                {Intl.NumberFormat("en-GB", { style: "currency", currency: "GBP" })
                  .format(Number(result.fCamaraCommissionAmount ?? result.totalFcamaraCommission ?? 0))}
              </td>
            </tr>
            <tr>
              <th scope="row">Total competitor commission</th>
              <td>
                {Intl.NumberFormat("en-GB", { style: "currency", currency: "GBP" })
                  .format(Number(result.competitorCommissionAmount ?? result.totalCompetitorCommission ?? 0))}
              </td>
            </tr>
          </tbody>
        </table>
      ) : (
        <p className="muted">Run a calculation to see results.</p>
      )}
    </div>
  </section>
</main>
  );
}

export default App;
