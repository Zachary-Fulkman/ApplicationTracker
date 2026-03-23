import { useEffect, useState } from "react";
import "./App.css";
import {
    getApplications,
    createApplication,
    type Application,
} from "./api/applicationApi";

function App() {
    const [applications, setApplications] = useState<Application[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [showForm, setShowForm] = useState(false);

    const [companyName, setCompanyName] = useState("");
    const [dateApplied, setDateApplied] = useState("");
    const [status, setStatus] = useState("");
    const [notes, setNotes] = useState("");

    async function loadApplications() {
        try {
            const result = await getApplications();
            setApplications(result.items);
        } catch {
            setError("Failed to load applications");
        } finally {
            setLoading(false);
        }
    }

    useEffect(() => {
        loadApplications();
    }, []);

    async function handleSubmit(event: React.FormEvent<HTMLFormElement>) {
        event.preventDefault();

        try {
            setError(null);

            await createApplication({
                companyName,
                dateApplied,
                status,
                notes,
            });

            setCompanyName("");
            setDateApplied("");
            setStatus("");
            setNotes("");
            setShowForm(false);

            await loadApplications();
        } catch {
            setError("Failed to create application");
        }
    }

    return (
        <div className="app">
            <header className="app-header">
                <h1 className="app-title">Job Applications</h1>
                <button
                    className="primary-button"
                    onClick={() => setShowForm((current) => !current)}
                >
                    {showForm ? "Close Form" : "Create Application"}
                </button>
            </header>

            {error && <p className="error-text">{error}</p>}

            {showForm && (
                <form className="form-panel" onSubmit={handleSubmit}>
                    <div className="form-grid">
                        <div className="form-field">
                            <label htmlFor="companyName">Company Name</label>
                            <input
                                id="companyName"
                                type="text"
                                value={companyName}
                                onChange={(event) => setCompanyName(event.target.value)}
                            />
                        </div>

                        <div className="form-field">
                            <label htmlFor="dateApplied">Date Applied</label>
                            <input
                                id="dateApplied"
                                type="date"
                                value={dateApplied}
                                onChange={(event) => setDateApplied(event.target.value)}
                            />
                        </div>

                        <div className="form-field">
                            <label htmlFor="status">Status</label>
                            <input
                                id="status"
                                type="text"
                                value={status}
                                onChange={(event) => setStatus(event.target.value)}
                            />
                        </div>

                        <div className="form-field full-width">
                            <label htmlFor="notes">Notes</label>
                            <textarea
                                id="notes"
                                value={notes}
                                onChange={(event) => setNotes(event.target.value)}
                            />
                        </div>
                    </div>

                    <div className="form-actions">
                        <button className="primary-button" type="submit">
                            Save Application
                        </button>
                        <button
                            className="secondary-button"
                            type="button"
                            onClick={() => setShowForm(false)}
                        >
                            Cancel
                        </button>
                    </div>
                </form>
            )}

            {loading ? (
                <p>Loading...</p>
            ) : applications.length === 0 ? (
                <p className="empty-state">No applications found.</p>
            ) : (
                <div className="application-grid">
                    {applications.map((app) => (
                        <div className="application-card" key={app.id}>
                            <h2>{app.companyName}</h2>
                            <p>
                                <strong>Status:</strong> {app.status}
                            </p>
                            <p>
                                <strong>Date Applied:</strong> {app.dateApplied}
                            </p>
                            <p>
                                <strong>Notes:</strong> {app.notes}
                            </p>

                            <div className="card-actions">
                                <button className="card-button" type="button">
                                    Update
                                </button>
                                <button className="card-button" type="button">
                                    Delete
                                </button>
                            </div>
                        </div>
                    ))}
                </div>
            )}
        </div>
    );
}

export default App;