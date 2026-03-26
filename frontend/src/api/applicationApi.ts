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

const BASE_URL = import.meta.env.VITE_API_BASE_URL;

export interface GetApplicationsParams {
    status?: string;
    company?: string;
    page?: number;
    pageSize?: number;
}

export async function getApplications(
    params?: GetApplicationsParams
): Promise<PagedResult<Application>> {
    const url = new URL(BASE_URL);

    if (params?.status) {
        url.searchParams.append("status", params.status);
    }

    if (params?.company) {
        url.searchParams.append("company", params.company);
    }

    if (params?.page) {
        url.searchParams.append("page", params.page.toString());
    }

    if (params?.pageSize) {
        url.searchParams.append("pageSize", params.pageSize.toString());
    }

    const response = await fetch(url.toString());

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

export async function deleteApplication(id: number) {
    const response = await fetch(`${BASE_URL}/${id}`, {
        method: "DELETE",
    });

    if (!response.ok) {
        throw new Error("Failed to delete application");
    }
}

export interface UpdateApplicationRequest {
    companyName: string;
    dateApplied: string;
    status: string;
    notes: string;
}

export async function updateApplication(
    id: number,
    data: UpdateApplicationRequest
) {
    const response = await fetch(`${BASE_URL}/${id}`, {
        method: "PUT",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify(data),
    });

    if (!response.ok) {
        throw new Error("Failed to update application");
    }
}