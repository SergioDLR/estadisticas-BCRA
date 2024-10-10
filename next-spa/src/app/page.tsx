import axios from 'axios'

export default async function Home() {
  const stats = await axios.get<{
    data: {
      idVariable: number
      cdSerie: number
      description: string
      dateValue: string
      value: number
      variation: number
    }[]
  }>('https://estadisticas-bcra-production.up.railway.app/statsbcra')

  return (
    <div className="grid grid-rows-[20px_1fr_20px] items-center justify-items-center min-h-screen p-8 pb-20 gap-16 sm:p-20 font-[family-name:var(--font-geist-sans)]">
      <h1>Principales variables</h1>
      {stats.data.data.map((stat) => (
        <div key={stat.idVariable}>
          <h5>{stat.description}</h5>
          <p>{stat.value}</p>
          <p>{stat.variation}</p>
        </div>
      ))}
    </div>
  )
}
