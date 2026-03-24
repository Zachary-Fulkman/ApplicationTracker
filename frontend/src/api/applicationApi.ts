export interface Application {
    id: number;
    companyName: string;
    dateApplied: string;
    status: string;
    notes: string;
}

export interface PagedResult<T> {
    items: T[];
    totalCount: number;
    page: number;
    pageSize: number;
}

const BASE_URL = "https://localhost:7031/api/application";

export async function getApplications(): Promise<PagedResult<Application>> {
    const response = await fetch(BASE_URL);

    if (!response.ok) {
        throw new Error("Failed to fetch applications");
    }

    return response.json();
}

export interface CreateApplicationRequest {
    companyName: string;
    dateApplied: string;
    status: string;
    notes: string;
}

export async function createApplication(data: CreateApplicationRequest) {
    const response = await fetch(BASE_URL, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify(data),
    });

    if (!response.ok) {
        throw new Error("Failed to create application");
    }

    return response.json();
}