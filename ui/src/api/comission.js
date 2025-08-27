const BASE_URL = process.env.REACT_APP_API_URL;

export async function calculateCommission(payload) {
  const res = await fetch(`${BASE_URL}/Commision`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(payload)
  });
  if (!res.ok) {
    const text = await res.text().catch(() => "");
    throw new Error(`HTTP ${res.status} ${res.statusText} ${text}`);
  }
  return res.json();
}