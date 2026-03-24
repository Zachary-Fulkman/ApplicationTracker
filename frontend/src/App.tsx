import { useEffect, useState } from "react";
import "./App.css";
import {
    getApplications,
    createApplication,
    deleteApplication,
    updateApplication,
    type Application,
} from "./api/applicationApi";

function App() {
    const [applications, setApplications] = useState<Application[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [showForm, setShowForm] = useState(false);
    const [editingAppId, setEditingAppId] = useState<number | null>(null);

    const [companyName, setCompanyName] = useState("");
    const [dateApplied, setDateApplied] = useState("");
    const [status, setStatus] = useState("");
    const [notes, setNotes] = useState("");

    const [statusFilter, setStatusFilter] = useState("");
    const [companyFilter, setCompanyFilter] = useState("");

    async function loadApplications() {
        try {
            const result = await getApplications({
                status: statusFilter,
                company: companyFilter,
            });
            setApplications(result.items);
        } catch {
            setError("Failed to load applications");
        } finally {
            setLoading(false);
        }
    }

    function handleEdit(app: Application) {
        setEditingAppId(app.id);
        setCompanyName(app.companyName);
        setDateApplied(app.dateApplied);
        setStatus(app.status);
        setNotes(app.notes);
        setShowForm(true);
        setError(null);
    }

    useEffect(() => {
        async function fetchData() {
            try {
                const result = await getApplications({
                    status: statusFilter,
                    company: companyFilter,
                });
                setApplications(result.items);
            } catch {
                setError("Failed to load applications");
            } finally {
                setLoading(false);
            }
        }

        fetchData();
    }, [statusFilter, companyFilter]);

    async function handleSubmit(event: React.FormEvent<HTMLFormElement>) {
        event.preventDefault();

        setError(null);

        if (!companyName.trim()) {
            setError("Company name is required");
            return;
        }

        if (!dateApplied.trim()) {
            setError("Date applied is required");
            return;
        }

        if (!status.trim()) {
            setError("Status is required");
            return;
        }

        try {
            setError(null);

            const formData = {
                companyName,
                dateApplied,
                status,
                notes,
            };

            if (editingAppId !== null) {
                await updateApplication(editingAppId, formData);
            } else {
                await createApplication(formData);
            }

            setCompanyName("");
            setDateApplied("");
            setStatus("");
            setNotes("");
            setEditingAppId(null);
            setShowForm(false);

            await loadApplications();
        } catch {
            setError(
                editingAppId !== null
                    ? "Failed to update application"
                    : "Failed to create application"
            );
        }
    }

    async function handleDelete(id: number) {
        const confirmed = window.confirm("Are you sure you want to delete this application?");
        if (!confirmed) return;
        try {
            await deleteApplication(id);
            await loadApplications();
        } catch {
            setError("Failed to delete application");
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
                            {editingAppId !== null ? "Update Application" : "Save Application"}
                        </button>
                        <button
                            className="secondary-button"
                            type="button"
                            onClick={() => {
                                setShowForm(false);
                                setEditingAppId(null);
                                setCompanyName("");
                                setDateApplied("");
                                setStatus("");
                                setNotes("");
                                setError(null);
                            }}
                        >
                            Cancel
                        </button>
                    </div>
                </form>
            )}

            <div className="filter-bar">
                <input
                    type="text"
                    placeholder="Search company..."
                    value={companyFilter}
                    onChange={(e) => setCompanyFilter(e.target.value)}
                />

                <select
                    value={statusFilter}
                    onChange={(e) => setStatusFilter(e.target.value)}
                >
                    <option value="">All Statuses</option>
                    <option value="Active">Active</option>
                    <option value="Applied">Applied</option>
                    <option value="Interview">Interview</option>
                    <option value="Rejected">Rejected</option>
                </select>
            </div>

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
                                <button
                                    className="card-button"
                                    type="button"
                                    onClick={() => handleEdit(app)}
                                >
                                    Update
                                </button>
                                <button
                                   className="card-button"
                                   type="button"
                                   onClick={() => handleDelete(app.id)}
                                >
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